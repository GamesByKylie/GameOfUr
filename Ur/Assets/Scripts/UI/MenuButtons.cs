using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{
    public static Action ResolutionChanged;

    public GameObject modalBlocker;
    public Dropdown resolutionDropdown;
    public GameObject[] extraScreens;
    public SettingsFromPlayerPrefs[] settingsOptions;

    private bool initialized = false;

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
        ToggleScreen(0, true);
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
    }

    public void CloseSettings()
    {
        ToggleScreen(0, false);
    }

    public void OpenHistory()
    {
        ToggleScreen(1, true);
    }

    public void CloseHistory()
    {
        ToggleScreen(1, false);
    }

    public void OpenCredits()
    {
        ToggleScreen(2, true);
    }

    public void CloseCredits()
    {
        ToggleScreen(2, false);
    }

    private void ToggleScreen(int screen, bool activate)
    {
        extraScreens[screen].SetActive(activate);
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
        SettingsManager.ScreenResolution = d.value;
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
        SettingsManager.InitializeSettings();
    }
}
