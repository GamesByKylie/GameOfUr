using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectorButton : MonoBehaviour
{
	public UrAIController.AIDifficulty difficulty;

	public void SetDifficulty() {
		if (GameManager.SelectedDifficulty != difficulty) {
			GameManager.SelectedDifficulty = difficulty;
		}
	}
}
