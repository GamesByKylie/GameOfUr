//David Herrod	
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class UrDiceRoller : MonoBehaviour
{
	public float skipTurnWaitTime = 1.5f;
    public float noAnimWaitTime = 0.5f;
    public float noAnimSpinTime = 0.15f;
	[Range(0f, 1f)] public float availableMoveBaseWeight = .25f;
	[Range(0f, 1f)] public float availableMoveWeightIncrease = 0.1f;
	public Text diceResultText;
	public float diceSpinTime;
	public Animator[] diceModels;
	public AudioClip[] diceSounds;
	public AudioSource diceSource;


	private UrGameController urGC;
	private float playerWeight;
	private float enemyWeight;

	private void Start() {
		urGC = GetComponent<UrGameController>();
		playerWeight = availableMoveBaseWeight;
		enemyWeight = availableMoveBaseWeight;
	}

	private IEnumerator RollAndRotate(Animator anim, string trigger)
	{
		//The reset trigger is needed so it goes back to idle for a frame and can then animate again
		//But if it gets called at a certain point, it could stay triggered when we don't want it to
		//which is why we call ResetTrigger
		yield return null;
		anim.SetTrigger("Reset");
		anim.transform.eulerAngles = new Vector3(0f, Random.Range(1f, 360f), 0f);
		anim.SetTrigger(trigger);

        if (diceSource.gameObject.activeSelf && SettingsManager.AnimationsEnabled)
        {
            diceSource.volume = SettingsManager.MasterVolume * SettingsManager.SFXVolume;
		    diceSource.clip = diceSounds[Random.Range(0, diceSounds.Length)];
            diceSource.Play();
        }
		yield return null;
		anim.ResetTrigger("Reset");
		yield return null;
		//Rotates at random y so the dice don't all look the same
		anim.transform.eulerAngles += Vector3.up * Random.Range(1f, 360f);
	}

	public int RollDice(bool playerTurn)
	{
		//1 is a blank, 2 is a mark
		int[] diceRolls = new int[diceModels.Length];

		for (int i = 0; i < diceRolls.Length; i++) 
		{
			diceRolls[i] = Random.Range(1, 3);
		}

		//Calculate - we're hard-coding it to 3 because there's not really a nice formula for the roll
		int marks = (diceRolls[0] % 2 == 0 ? 1 : 0) + (diceRolls[1] % 2 == 0 ? 1 : 0) + (diceRolls[2] % 2 == 0 ? 1 : 0);
		int roll = marks == 3 ? 5 : marks;

		//If you can't move
		if (roll == 0 || !urGC.CanPlayerMove(playerTurn, roll, false)) {
			//Check if you reach the weight to reroll for a better roll
			float rand = Random.Range(0f, 1f);
			float threshold = playerTurn ? playerWeight : enemyWeight;

			//If you do qualify for a reroll
			if (rand < threshold) {
				//Keep rerolling until you get a move that works
				do {
					for (int i = 0; i < diceRolls.Length; i++) {
						diceRolls[i] = Random.Range(1, 3);
					}

					marks = (diceRolls[0] % 2 == 0 ? 1 : 0) + (diceRolls[1] % 2 == 0 ? 1 : 0) + (diceRolls[2] % 2 == 0 ? 1 : 0);
					roll = marks == 3 ? 5 : marks;

				} while (!urGC.CanPlayerMove(playerTurn, roll, false));

				//Make sure to reset the weight
				if (playerTurn) {
					playerWeight = availableMoveBaseWeight;
				}
				else {
					enemyWeight = availableMoveBaseWeight;
				}
			}
			//If you don't, keep this roll but increase the weight so you're less likely to miss a turn again
			else {
				if (playerTurn) {
					playerWeight += availableMoveWeightIncrease;
				}
				else {
					enemyWeight += availableMoveWeightIncrease;
				}
			}
		}
		else {
			//Reset the weights here as well - we only want it increasing for bad rolls in a row
			if (playerTurn) {
				playerWeight = availableMoveBaseWeight;
			}
			else {
				enemyWeight = availableMoveBaseWeight;
			}
		}

		//Once the roll is finalized, then send it on to the visuals
		if (!urGC.IsGameOver) 
		{
			StartCoroutine(VisualDiceRoll(diceRolls, roll, playerTurn));
		}
		
		return roll;
	}

	private IEnumerator VisualDiceRoll(int[] diceRolls, int resultRoll, bool playerTurn) 
	{
		//Visually rotate the dice so they look like they're rolling
		//I TRIED to do it procedurally by adding to the transform.rotation, but it wouldn't work
		//First I tried Euler angles and kept having issues with them not being unique and with gimble lock
		//Then I tried Transform.Rotate, but sometimes it would cause massive framerate dips
		//So I had to give up and do animations instead

		for (int i = 0; i < diceModels.Length; i++) 
		{
			int suffix = Random.Range(1, 3);
			//1 is blank, 2 is mark
			string trigger;
            if (SettingsManager.AnimationsEnabled)
            {
                trigger = diceRolls[i] == 1 ? "RollBlank" : "RollMark";
            }
            else
            {
                trigger = diceRolls[i] == 1 ? "JumpBlank" : "JumpMark";
            }

			trigger += suffix.ToString();

			StartCoroutine(RollAndRotate(diceModels[i], trigger));
		}

    	yield return new WaitForSeconds(SettingsManager.AnimationsEnabled ? diceSpinTime : noAnimSpinTime);
        
		diceResultText.text = resultRoll.ToString();

		if (playerTurn) 
		{
			if (!urGC.CanPlayerMove(true)) 
			{
				urGC.ShowAlertText("No Available Moves");
				urGC.PlaySoundFX(UrGameController.SoundTrigger.LostTurn, true);
                StartCoroutine(urGC.WaitToSwitchTurn(false, true, SettingsManager.AnimationsEnabled ? skipTurnWaitTime : noAnimWaitTime));
			}
		}
		else 
		{
			if (!urGC.CanPlayerMove(false, false)) 
			{
				urGC.ShowAlertText("Opponent Has No Moves");
				urGC.PlaySoundFX(UrGameController.SoundTrigger.LostTurn, false);
                StartCoroutine(urGC.WaitToSwitchTurn(true, true, SettingsManager.AnimationsEnabled ? skipTurnWaitTime : noAnimWaitTime));
			}
			else 
			{
				StartCoroutine(urGC.enemyAI.DoEnemyTurn());
			}
		}
	}

    public IEnumerator RollVisualsOnly()
    {
        while (true)
        {
            //Determine numbers
            int[] diceRolls = new int[diceModels.Length];

            for (int i = 0; i < diceRolls.Length; i++)
            {
                diceRolls[i] = Random.Range(1, 3);
            }

            //Calculate - we're hard-coding it to 3 because there's not really a nice formula for the roll
            int marks = (diceRolls[0] % 2 == 0 ? 1 : 0) + (diceRolls[1] % 2 == 0 ? 1 : 0) + (diceRolls[2] % 2 == 0 ? 1 : 0);
            int roll = marks == 3 ? 5 : marks;

            //Then do the visuals
            for (int i = 0; i < diceModels.Length; i++)
            {
                int suffix = Random.Range(1, 3);
                //1 is blank, 2 is mark
                string trigger = diceRolls[i] == 1 ? "RollBlank" : "RollMark";
                trigger += suffix.ToString();
                StartCoroutine(RollAndRotate(diceModels[i], trigger));
            }

            yield return new WaitForSeconds(diceSpinTime);

            diceResultText.text = roll.ToString();

            yield return new WaitForSeconds(1.5f);
        }

    }
}


