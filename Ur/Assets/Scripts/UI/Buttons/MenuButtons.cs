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
	public MenuPanelAnimator menuBox;
}

public class MenuButtons : MonoBehaviour
{
	public static Action PlayerSelection;
    public static Action ResolutionChanged;

    public GameObject modalBlocker;
    public Button resolutionConfirmButton;
    public Text resolutionConfirmText;
    public Dropdown resolutionDropdown;

    public SettingsFromPlayerPrefs[] settingsOptions;
	
    protected bool initialized = false;
    protected Coroutine resolutionConfirmRoutine;
    protected int previousResolution;

	public void LoadMainMenu() {
        GameManager.LoadMainMenu();
    }

    public void LoadGamePlay() {
        GameManager.LoadGamePlay();
    }

	public void ExitGame() {
        Application.Quit();
    }

    public void InitializeSettings() {
        StartCoroutine(DoInitializeSettings());
    }

    protected IEnumerator DoInitializeSettings() {
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
}
