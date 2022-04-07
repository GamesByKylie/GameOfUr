using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject modalBlocker;
    public GameObject[] extraScreens;
    public SettingsFromPlayerPrefs[] settingsOptions;

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
        foreach (var s in settingsOptions)
        {
            s.SetValueFromPref();
        }
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

    public void UpdateAnimationsEnabled(bool enabled)
    {
        SettingsManager.AnimationsEnabled = enabled;
    }
}
