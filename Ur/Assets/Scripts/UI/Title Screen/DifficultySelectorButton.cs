using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectorButton : MonoBehaviour
{
	public UrAIController.AIDifficulty difficulty;

	private Toggle tog;

	private void Start() {
		tog = GetComponent<Toggle>();
	}

	public void SetDifficulty(bool toggle) {
		if (toggle) {
			if (tog.group.allowSwitchOff) {
				tog.group.allowSwitchOff = false;
			}
			if (GameManager.SelectedDifficulty != difficulty) {
				GameManager.SelectedDifficulty = difficulty;
				GameManager.PlayButtonClick();
				MenuButtons.PlayerSelection?.Invoke();
			}
		}
	}
}
