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
    public static Action ResolutionChanged;

    public GameObject modalBlocker;
    public Button resolutionConfirmButton;
    public Text resolutionConfirmText;
    public Dropdown resolutionDropdown;
    public MenuScreen[] extraScreens;
    public SettingsFromPlayerPrefs[] settingsOptions;

    private bool initialized = false;
    private Coroutine resolutionConfirmRoutine;
    private int previousResolution;

    public void LoadMainMenu()
    {
        GameManager.LoadMainMenu();
    }

    public void LoadGamePlay()
    {
        GameManager.LoadGamePlay();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        ToggleScreen(MenuScreen.ScreenType.Settings, true);
        if (!initialized)
        {
            InitializeSettings();
        }
    }

    public void InitializeSettings()
    {
        StartCoroutine(DoInitializeSettings());
    }

    private IEnumerator DoInitializeSettings()
    {
        resolutionDropdown.ClearOptions();
        foreach (var r in SettingsManager.AvailableResolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(r.width + "x" + r.height));
        }

        yield return null;

        foreach (var s in settingsOptions)
        {
            s.SetValueFromPref();
        }

        initialized = true;
        previousResolution = SettingsManager.ScreenResolution;
    }

    public void OpenCharacterSelection()
    {
        ToggleScreen(MenuScreen.ScreenType.CharacterSelector, true);
    }

    public void CloseCharacterSelection()
    {
        ToggleScreen(MenuScreen.ScreenType.CharacterSelector, false);
    }

    public void CloseSettings()
    {
        ToggleScreen(MenuScreen.ScreenType.Settings, false);
    }

    public void OpenHistory()
    {
        ToggleScreen(MenuScreen.ScreenType.History, true);
    }

    public void CloseHistory()
    {
        ToggleScreen(MenuScreen.ScreenType.History, false);
    }

    public void OpenCredits()
    {
        ToggleScreen(MenuScreen.ScreenType.Credits, true);
    }

    public void CloseCredits()
    {
        ToggleScreen(MenuScreen.ScreenType.Credits, false);
    }

	public void OpenStats()
	{
		ToggleScreen(MenuScreen.ScreenType.Stats, true);
	}

	public void CloseStats()
	{
		ToggleScreen(MenuScreen.ScreenType.Stats, false);
	}

    private void ToggleScreen(MenuScreen.ScreenType type, bool activate)
    {
        extraScreens.FirstOrDefault(x => x.type == type).screenObj.SetActive(activate);
        modalBlocker.SetActive(activate);
    }

    public void UpdateMasterVolume(System.Single volume)
    {
        SettingsManager.MasterVolume = volume;
    }

    public void UpdateSFXVolume(System.Single volume)
    {
        SettingsManager.SFXVolume = volume;
    }

    public void UpdateMusicVolume(System.Single volume)
    {
        SettingsManager.MusicVolume = volume;
    }

    public void UpdateScreenResolution(Dropdown d)
    {
        previousResolution = SettingsManager.ScreenResolution;
        SettingsManager.ScreenResolution = d.value;
        resolutionConfirmRoutine = StartCoroutine(DoConfirmResolution());
    }

    public void ConfirmResolution()
    {
        StopCoroutine(resolutionConfirmRoutine);
        resolutionConfirmButton.interactable = false;
        resolutionConfirmText.text = "";
    }

    private IEnumerator DoConfirmResolution()
    {
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

    public void UpdateAnimationsEnabled(bool enabled)
    {
        SettingsManager.AnimationsEnabled = enabled;
    }

    public void UpdateFullscreen(bool enabled)
    {
        SettingsManager.Fullscreen = enabled;
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
		SaveManager.RestoreDefaults();
        SettingsManager.InitializeSettings();
    }
}
