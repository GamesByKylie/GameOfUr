using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;

[Serializable]
public class MenuScreen
{
    public enum ScreenType { CharacterSelector, Settings, History, Credits, Stats }

    public ScreenType type;
    public GameObject screenObj;
}

public class MenuButtons : MonoBehaviour
{
	public static Action CharacterSelected;
    public static Action ResolutionChanged;

    public GameObject modalBlocker;
    public Button resolutionConfirmButton;
    public Text resolutionConfirmText;
    public Dropdown resolutionDropdown;
    public MenuScreen[] extraScreens;
    public SettingsFromPlayerPrefs[] settingsOptions;
	public Button[] difficultyButtons;

    private bool initialized = false;
    private Coroutine resolutionConfirmRoutine;
    private int previousResolution;

	private void OnEnable() {
		CharacterSelected += EnableGameStart;
	}

	private void OnDisable() {
		CharacterSelected -= EnableGameStart;
	}

	public void LoadMainMenu() {
        GameManager.LoadMainMenu();
    }

    public void LoadGamePlay() {
		Debug.Log($"Starting with difficulty {GameManager.SelectedDifficulty}");
        GameManager.LoadGamePlay();
    }

	private void SetDifficulty(UrAIController.AIDifficulty difficulty) {
		GameManager.SelectedDifficulty = difficulty;
	}
	public void SetDifficultyEasy() {
		SetDifficulty(UrAIController.AIDifficulty.Easy);
	}
	public void SetDifficultyMedium() {
		SetDifficulty(UrAIController.AIDifficulty.Medium);
	}
	public void SetDifficultyHard() {
		SetDifficulty(UrAIController.AIDifficulty.Hard);
	}

	public void ExitGame() {
        Application.Quit();
    }

    public void InitializeSettings() {
        StartCoroutine(DoInitializeSettings());
    }

    private IEnumerator DoInitializeSettings() {
        resolutionDropdown.ClearOptions();
        foreach (var r in SettingsManager.AvailableResolutions) {
            resolutionDropdown.options.Add(new Dropdown.OptionData(r.width + "x" + r.height));
        }

        yield return null;

        foreach (var s in settingsOptions) {
            s.SetValueFromPref();
        }

        initialized = true;
        previousResolution = SettingsManager.ScreenResolution;
    }

	public void CharacterSelectionEnabled(bool enabled) {
		MenuScreenEnabled(MenuScreen.ScreenType.CharacterSelector, enabled);
	}

	public void SettingsEnabled(bool enabled) {
		MenuScreenEnabled(MenuScreen.ScreenType.Settings, enabled);
		if(enabled && !initialized) {
			InitializeSettings();
		}
	}

	public void HistoryEnabled(bool enabled) {
		MenuScreenEnabled(MenuScreen.ScreenType.History, enabled);
	}

	public void CreditsEnabled(bool enabled) {
		MenuScreenEnabled(MenuScreen.ScreenType.Credits, enabled);
	}

    public void StatsEnabled(bool enabled) {
		MenuScreenEnabled(MenuScreen.ScreenType.Stats, enabled);
	}

    private void MenuScreenEnabled(MenuScreen.ScreenType type, bool activate) {
        extraScreens.FirstOrDefault(x => x.type == type).screenObj.SetActive(activate);
        modalBlocker.SetActive(activate);
    }

    public void UpdateMasterVolume(float volume) {
        SettingsManager.MasterVolume = volume;
    }

    public void UpdateSFXVolume(float volume) {
        SettingsManager.SFXVolume = volume;
    }

    public void UpdateMusicVolume(float volume) {
        SettingsManager.MusicVolume = volume;
    }

    public void UpdateScreenResolution(Dropdown d) {
        previousResolution = SettingsManager.ScreenResolution;
        SettingsManager.ScreenResolution = d.value;
        resolutionConfirmRoutine = StartCoroutine(DoConfirmResolution());
    }

    public void ConfirmResolution() {
        StopCoroutine(resolutionConfirmRoutine);
        resolutionConfirmButton.interactable = false;
        resolutionConfirmText.text = "";
    }

    private IEnumerator DoConfirmResolution() {
        resolutionConfirmButton.interactable = true;
        resolutionConfirmText.text = "Confirm? 3";
        yield return new WaitForSeconds(1);
        resolutionConfirmText.text = "Confirm? 2";
        yield return new WaitForSeconds(1);
        resolutionConfirmText.text = "Confirm? 1";
        yield return new WaitForSeconds(1);
        SettingsManager.ScreenResolution = previousResolution;
        resolutionDropdown.value = previousResolution;
        resolutionConfirmButton.interactable = false;
        resolutionConfirmText.text = "";
    }

    public void UpdateAnimationsEnabled(bool enabled) {
        SettingsManager.AnimationsEnabled = enabled;
    }

    public void UpdateFullscreen(bool enabled) {
        SettingsManager.Fullscreen = enabled;
    }

    public void ResetPlayerPrefs() {
        PlayerPrefs.DeleteAll();
		SaveManager.RestoreDefaults();
        SettingsManager.InitializeSettings();
    }

	private void EnableGameStart() {
		Debug.Log($"Toggling start buttons: {(GameManager.SelectedCharacter != null ? GameManager.SelectedCharacter.characterName : "null")}");
		foreach(var b in difficultyButtons) {
			b.interactable = GameManager.SelectedCharacter != null;
		}
	}
}
