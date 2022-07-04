//David Herrod
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UrGameController : MonoBehaviour
{
	[Header("Player")]
	public string playerTag;
	public List<UrPiece> playerPieces;
	public List<UrGameTile> playerBoardPositions;
	public GameObject playerPathLine;

	[Header("Enemy")]
	public string enemyTag;
	[HideInInspector] public UrAIController enemyAI;
	public List<UrGameTile> enemyBoardPositions;
	public GameObject enemyPathLine;

	[Header("UI")]
	public Camera mainCam;
    public GameObject startUI;
    public GameObject gameOverUI;
    public Text gameOverText;
    public TextDisplayBox displayBox;
    public GameObject modalBlocker;
    public Animator pauseMenuAnim;
	//[Range(0f, 1f)]
	//public float bragToInsultRatio = 0.66f;
	public Button rollDiceButton;
    public Text turnText;
	public Text alertText;
	public float alertShowTime;
	public float alertFadeSpeed;

	[Header("Audio")]
	public AudioSource sfxAud;
    public AudioSource moveSound;
	public AudioClip[] rosetteSounds;
	public AudioClip[] captureSounds;
	public AudioClip[] lostTurnSounds;
	public AudioClip[] offBoardSounds;

    [Header("Tutorial")]
    public List<TutorialInfo> tutorial;

	private bool isGameOver = false;
	private int currentRoll;
	private bool isPlayerTurn = true;
	private bool allowPlayerMove = false;

	private UrDiceRoller dice;
    private MenuButtons menuButtons;

    private int turnCount = 1;

	private Color baseAlertColor;
	private Outline alertOutline;
	private Color baseOutlineColor;
	private Coroutine fadeCoroutine;

    private bool waitingForInput = false;
    private int tutorialIndex = 0;

    private bool gamePaused = false;
    private bool settingsInitialized = false;

    [HideInInspector] public GameManager gm;

	public void Awake() {
		//Assign variables
		enemyAI = GetComponent<UrAIController>();
		dice = GetComponent<UrDiceRoller>();
        gm = GameObject.FindWithTag("Master").GetComponent<GameManager>();
        menuButtons = GetComponent<MenuButtons>();

        playerPathLine.SetActive(false);
        enemyPathLine.SetActive(false);

		//Set up the baseline for the alert colors
		baseAlertColor = alertText.color;
		alertOutline = alertText.GetComponent<Outline>();
		baseOutlineColor = alertOutline.effectColor;
		alertText.text = "";

        displayBox.gameObject.SetActive(false);

        //ask if they want a tutorial
        if (!PlayerPrefs.HasKey("played_before"))
        {
            Debug.Log("Player's first time - tutorial offer");
            modalBlocker.SetActive(true);
            startUI.SetActive(true);
            PlayerPrefs.SetString("played_before", "true");
        }

	}

    public void PauseMinigame() {
        gamePaused = !gamePaused;
        if (!gamePaused) {
            Time.timeScale = 1f;
        }
        pauseMenuAnim.SetTrigger("PauseMenuisActive");
        modalBlocker.SetActive(gamePaused);
        if (!settingsInitialized) {
            settingsInitialized = true;
            menuButtons.InitializeSettings();
        }

	}

    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    //public void UnpauseMinigame()
    //{
    //    Time.timeScale = 1f;
    //    pauseMenuAnim.SetBool("PauseMenuisActive", false);
    //    modalBlocker.SetActive(false);
    //}

    public void PlayMoveSound()
    {
        moveSound.volume = SettingsManager.MasterVolume * SettingsManager.SFXVolume;
        moveSound.pitch = Random.Range(0.7f, 1.1f);
        moveSound.Play();
    }

    public void RestartScene()
    {
        GameManager.LoadGamePlay();
    }

    public void ExitGameplay()
    {
        GameManager.LoadMainMenu();
    }

    public void RestartTutorial()
    {
        //Whoops, how are we going to show all the nice little animations when we don't know the state of the board?
        //We don't want pieces on top of each other, so we might need to temporarily turn off all the real pieces
        //And then turn on some example dummy pieces that go away once the tutorial is completed
        //And what if this happens mid-turn while the enemy is moving?
        //We don't want the player to interrupt either turn, it could be used for exploits maybe
        //So grey it out while it's not their turn, maybe?
        //Also timescale has to be set to 1 so the animations will run!
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.anyKeyDown)
            {
                if (tutorialIndex < tutorial.Count - 1)
                {
                    DisplayNextTutorial();
                }
                else if (tutorialIndex == tutorial.Count - 1)
                {
                    FinishTutorial();
                }
            }
        }
    }

    public void StartTutorial()
    {
        tutorialIndex = 0;
        displayBox.gameObject.SetActive(true);
        startUI.gameObject.SetActive(false);
        displayBox.DisplayMessage(tutorial[0].text, tutorial[0].anchorMin, tutorial[0].anchorMax);
        TutorialStartObjects(tutorialIndex);
        waitingForInput = true;
        turnText.text = "Press any key to continue";
    }

    public void DisplayNextTutorial()
    {
        TutorialEndObjects(tutorialIndex);
        tutorialIndex++;

        if (tutorialIndex < tutorial.Count)
        {
            TutorialStartObjects(tutorialIndex);
            displayBox.DisplayMessage(tutorial[tutorialIndex].text, tutorial[tutorialIndex].anchorMin, tutorial[tutorialIndex].anchorMax);
        }
    }

    private void TutorialStartObjects(int i)
    {
        foreach (GameObject g in tutorial[i].turnOnAtStart)
        {
            g.SetActive(true);
        }

        foreach (GameObject g in tutorial[i].turnOffAtStart)
        {
            g.SetActive(false);
        }
    }

    private void TutorialEndObjects(int i)
    {
        foreach (GameObject g in tutorial[i].turnOnAtEnd)
        {
            g.SetActive(true);
        }

        foreach (GameObject g in tutorial[i].turnOffAtEnd)
        {
            g.SetActive(false);
        }
    }

    public void FinishTutorial()
    {
        waitingForInput = false;
        modalBlocker.SetActive(false);
        displayBox.gameObject.SetActive(false);
        playerPathLine.SetActive(true);
        turnText.text = "Turn 0";
    }

    /// <summary>
    /// Turns off all board tile highlights
    /// </summary>
    public void UnhighlightBoard() {
		foreach (UrGameTile tile in playerBoardPositions) {
			tile.ShowHighlight(false);
		}

		foreach (UrGameTile tile in enemyBoardPositions) {
			tile.ShowHighlight(false);
		}
	}

	public void UnhighlightPieces() {
		foreach (UrPlayerPiece piece in playerPieces) {
			if (piece != null) {
				piece.ShowHighlight(false);
			}

		}
		foreach (UrPiece piece in enemyAI.enemyPieces) {
			if (piece != null) {
				piece.ShowHighlight(false);
			}

		}
	}

	/// <summary>
	/// Rolls the dice and returns the result
	/// </summary>
	/// <returns></returns>
	public int GetDiceRoll() {
		rollDiceButton.interactable = false;
		currentRoll = dice.RollDice(isPlayerTurn);

		return currentRoll;
	}

	/// <summary>
	/// Rolls the dice - used for the button
	/// </summary>
	public void RollDice() {
		//Prevents button from being pressed if there's a bark onscreen
		if (Time.timeScale != 0) {
			rollDiceButton.interactable = false;
			currentRoll = dice.RollDice(isPlayerTurn);
			if (isPlayerTurn) {
				allowPlayerMove = true;
			}
		}
	}

	public void SwitchTurn(bool playerTurn, bool updateTurn) {
		isPlayerTurn = playerTurn;
		allowPlayerMove = false;
		if (!isGameOver) 
		{
			rollDiceButton.interactable = isPlayerTurn;
            if (updateTurn)
            {
                turnCount++;
                turnText.text = "Turn " + turnCount;
            }
		}


		playerPathLine.SetActive(isPlayerTurn);
		enemyPathLine.SetActive(!isPlayerTurn);

		UnhighlightBoard();
		UnhighlightPieces();

		//The thought with changing the scale slightly is for overlaps when it comes to capturing
		//Sometimes, the player would always show through, but sometimes they'd show half-and-half
		//I want whichever piece that's doing the capturing to show on top, so I figured
		//let's make the pieces being captured (ie whoever's turn it's not) a tiny bit smaller so they don't overlap
		//It sounds dumb, but it does actually work, and you can't see the scale change unless you're really looking for it
		//The other option was fiddling with camera layers or something and this game does enough of that already!

        //TODO: change this to using the layers or whatever, I've started being able to see the tiny size changes and it's really annoying me
		if (!isPlayerTurn) {
			foreach (var p in enemyAI.enemyPieces) {
				p.transform.localScale = Vector3.one;
			}
			foreach (var p in playerPieces) {
				p.transform.localScale = Vector3.one * 0.99f;
			}
			enemyAI.EnemyTurn();
		}
		else {
			foreach (var p in playerPieces) {
				p.transform.localScale = Vector3.one;
			}
			foreach (var p in enemyAI.enemyPieces) {
				p.transform.localScale = Vector3.one * 0.99f;
			}
		}

	}

	public IEnumerator WaitToSwitchTurn(bool playerTurn, bool updateTurn, float waitTime) {
		yield return new WaitForSeconds(waitTime);
		SwitchTurn(playerTurn, updateTurn);
	}

	public bool CanPlayerMove(bool isPlayer, bool highlightPieces = true) {
		return CanPlayerMove(isPlayer, currentRoll, highlightPieces);
	}

	/// <summary>
	/// Checks if the specified player can move any of their pieces
	/// </summary>
	/// <param name="isPlayer">Whether to check the player or not</param>
	/// <param name="highlightPieces">Whether to highlight any mobile pieces</param>
	/// <returns></returns>
	public bool CanPlayerMove(bool isPlayer, int roll, bool highlightPieces = true) {
		int movable = 0;
		List<UrGameTile> checkPath = new List<UrGameTile>();
		List<UrPiece> checkPieces = new List<UrPiece>();

		if (isPlayer) {
			checkPath = playerBoardPositions;
			checkPieces = playerPieces;
		}
		else {
			checkPath = enemyBoardPositions;
			checkPieces = enemyAI.enemyPieces;
		}

		foreach (UrPiece p in checkPieces) {
			if (p.PopulateValidMovesList(checkPath, isPlayer, roll).Count > 0) {
				if (highlightPieces) {
					p.ShowHighlight(true);
				}

				movable++;
			}
		}

		return movable > 0;
	}

	public void ShowAlertText(string alert) {
		StartCoroutine(DoShowAlertText(alertText, alertOutline, alert));
	}

	private IEnumerator DoShowAlertText(Text t, Outline o, string alert) {
		//For some reason, just calling StopCoroutine(FadeText(t, o)) doesn't work, so we have to do it this way
		if (fadeCoroutine != null) {
			StopCoroutine(fadeCoroutine);
			fadeCoroutine = null;
		}
		yield return null;
		t.color = baseAlertColor;
		o.effectColor = baseOutlineColor;
		alertText.text = alert;
		yield return null;
		fadeCoroutine = StartCoroutine(FadeText(t, o));
	}

	private IEnumerator FadeText(Text t, Outline o) {
		yield return new WaitForSeconds(alertShowTime);
		Color clearColor = new Color(baseAlertColor.r, baseAlertColor.g, baseAlertColor.b, 0f);
		Color clearOutline = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, 0f);

		//I had a lot of trouble with this for some reason - Lerp didn't want to cooperate
		//I've seen people do something like Color.Lerp(t.color, endColor, t) with t as the for loop iterater,
		//but for some reason that wasn't giving the right results here
		for (float i = 0; i <= 1; i += Time.deltaTime * alertFadeSpeed) {
			t.color = Color.Lerp(baseAlertColor, clearColor, i);
			o.effectColor = Color.Lerp(baseOutlineColor, clearOutline, i);
			yield return null;
		}

		alertText.text = "";
		alertText.color = baseAlertColor;
		o.effectColor = baseOutlineColor;
	}

	//public void TriggerBark(bool isPlayer, List<string> triggerType, bool autoTrigger = false)
 //   {
	//	float rand = Random.Range(0f, 1f);

	//	//If we want this to disregard the random element, just manually set it to 0 so it's always below barkChance
	//	if (autoTrigger) {
	//		rand = 0f;
	//	}

	//	//If you've actually triggered one, you can either do the corresponding brag or an insult
	//	if (rand <= barkChance) {
	//		if (Random.Range(0f, 1f) <= bragToInsultRatio || autoTrigger) {
	//			//If the player did the cool thing, the player brags
	//			if (isPlayer) {
	//				playerBarks.DisplayFromList(triggerType);
	//			}
	//			//Otherwise, the enemy brags
	//			else {
	//				enemyBarks.DisplayFromList(triggerType);
	//			}
	//		}
	//		else {
	//			//If you're going to do the insult instead, it's the opposite
	//			if (isPlayer) {
	//				enemyBarks.DisplayInsult();
	//			}
	//			else {
	//				playerBarks.DisplayInsult();
	//			}
	//		}
	//	}
	//}

	public void PointScored(bool isPlayer, UrPiece c) {
		if (isPlayer) {
			playerPieces.Remove(c);
			c.GetComponent<MeshRenderer>().enabled = false;
			Destroy(c.gameObject, 1f);
			if (playerPieces.Count == 0) {
				WinGame();
			}
		}
		else {
			enemyAI.enemyPieces.Remove(c);
			c.GetComponent<MeshRenderer>().enabled = false;
			Destroy(c.gameObject, 1f);
			Debug.Log("Enemy point scored, remaining pieces " + enemyAI.enemyPieces.Count);
			if (enemyAI.enemyPieces.Count == 0) {
				Debug.Log("Ending the game - enemy win");
				LoseGame();
			}
		}
	}

	public enum SoundTrigger {Rosette, Capture, LostTurn, OffBoard };
	public void PlaySoundFX(SoundTrigger type, bool isPlayer) 
	{
		AudioClip[] sounds = null;

		switch (type) {
			case (SoundTrigger.Rosette):
				sounds = rosetteSounds;
				break;
			case (SoundTrigger.Capture):
				sounds = captureSounds;
				break;
			case (SoundTrigger.LostTurn):
				sounds = lostTurnSounds;
				break;
			case (SoundTrigger.OffBoard):
				sounds = offBoardSounds;
				break;
		}

        sfxAud.clip = isPlayer ? sounds[0] : sounds[1];
        sfxAud.volume = SettingsManager.MasterVolume * SettingsManager.SFXVolume;
		sfxAud.Play();
	}

	private void WinGame() 
	{
		isGameOver = true;
		rollDiceButton.interactable = false;
		allowPlayerMove = false;

        gameOverUI.SetActive(true);
        modalBlocker.SetActive(true);
        gameOverText.text = $"<size=60><b>Congratulations!</b></size>\n\nTotal Turns: {turnCount}\n\n";
        //if current turns is lower than best score, append <b>New High Score!</b>
	}

	private void LoseGame() 
	{
		isGameOver = false;
		rollDiceButton.interactable = false;
		allowPlayerMove = false;

        gameOverUI.SetActive(true);
        modalBlocker.SetActive(true);
        gameOverText.text = $"<size=60><b>Too Bad!</b></size>\nTotal Turns: {turnCount}\n\n";
	}

	public int CurrentRoll {
		get {
			return currentRoll;
		}
	}

	public bool IsPlayerTurn {
		get {
			return isPlayerTurn;
		}
	}

	public bool AllowPlayerMove {
		get {
			return allowPlayerMove;
		}
	}

	public bool IsGameOver {
		get {
			return isGameOver;
		}
	}
}
