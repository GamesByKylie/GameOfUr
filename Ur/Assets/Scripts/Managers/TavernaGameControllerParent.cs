using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernaGameControllerParent : MonoBehaviour
{
	public AudioSource moveSound;

	[Header("Main UI")]
	public TavernaMiniGameDialog playerBarks;
	public TavernaEnemyDialog enemyBarks;
	[Range(0f, 1f)]
	public float barkChance = 0.75f;


	public virtual void PauseMinigame() {
		//mgScreen.gameObject.SetActive(true);
		Time.timeScale = 0;
	}

	public virtual void UnpauseMinigame() {
		//mgScreen.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	/// <summary>
	/// Plays the movement sound at a random pitch
	/// </summary>
	public void PlayMoveSound() {
        moveSound.volume = SettingsManager.MasterVolume * SettingsManager.SFXVolume;
        moveSound.pitch = Random.Range(0.7f, 1.1f);
		moveSound.Play();
	}
}
