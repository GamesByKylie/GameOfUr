using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenButtons : MenuButtons
{
	public MenuScreen[] extraScreens;
	public Button gameStartButton;

	private void OnEnable() {
		PlayerSelection += EnableGameStart;

		GameManager.SelectedCharacter = null;
		GameManager.SelectedDifficulty = UrAIController.AIDifficulty.None;
		gameStartButton.interactable = false;
	}

	private void OnDisable() {
		PlayerSelection -= EnableGameStart;
	}

	public void EnableCharacterSelecction(bool enabled) {
		EnableMenuScreen(MenuScreen.ScreenType.CharacterSelector, enabled);
	}

	public void EnableSettings(bool enabled) {
		EnableMenuScreen(MenuScreen.ScreenType.Settings, enabled);
		if (enabled && !initialized) {
			InitializeSettings();
		}
	}

	public void EnableHistory(bool enabled) {
		EnableMenuScreen(MenuScreen.ScreenType.History, enabled);
	}

	public void EnableCredits(bool enabled) {
		EnableMenuScreen(MenuScreen.ScreenType.Credits, enabled);
	}

	public void EnableStats(bool enabled) {
		EnableMenuScreen(MenuScreen.ScreenType.Stats, enabled);
	}

	private void EnableMenuScreen(MenuScreen.ScreenType type, bool activate) {
		extraScreens.FirstOrDefault(x => x.type == type).menuBox.EnableAnimation(activate);
		modalBlocker.SetActive(activate);
	}

	private void EnableGameStart() {
		if (GameManager.SelectedCharacter != null && GameManager.SelectedDifficulty != UrAIController.AIDifficulty.None) {
			gameStartButton.interactable = true;
		}
	}

	public void ResetPlayerPrefs() {
		PlayerPrefs.DeleteAll();
		SaveManager.RestoreDefaults();
		SettingsManager.InitializeSettings();
	}
}
