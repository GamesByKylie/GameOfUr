using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SettingsManager : MonoBehaviour
{
    private const float DEFAULT_MASTER_VOLUME = 1.0f;
    private const float DEFAULT_SFX_VOLUME = 0.75f;
    private const float DEFAULT_MUSIC_VOLUME = 0.5f;

    private const int DEFAULT_RESOLUTION = 0;

    private const bool DEFAULT_ANIMATIONS = true;
    private const bool DEFAULT_FULLSCREEN = true;

    private static float masterVolume;
    private static float sfxVolume;
    private static float musicVolume;
    private static int screenResolution;
    private static bool animationsEnabled;
    private static bool fullscreen;

    private static AudioSource bgmSource;
    private static Resolution[] supportedResolutions = new Resolution[] {
        new Resolution{ width = 1280, height = 720 },
        new Resolution{ width = 1360, height = 768 },
        new Resolution{ width = 1366, height = 768 },
        new Resolution{ width = 1600, height = 900 },
        new Resolution{ width = 1920, height = 1080 },
        new Resolution{ width = 1920, height = 1200 },
        new Resolution{ width = 2560, height = 1600 },
        new Resolution{ width = 2560, height = 1440 },
        new Resolution{ width = 2560, height = 1080 },
        new Resolution{ width = 3440, height = 1440 },
        new Resolution{ width = 3840, height = 2160 }
        };
    private static List<Resolution> availableRes = new List<Resolution>();

    private void Awake()
    {
        //List<Resolution> potentialRes = Screen.resolutions.Select(r => new Resolution { width = r.width, height = r.height }).Distinct().ToList();
		//foreach (var r in potentialRes) {
		//    foreach (var s in supportedResolutions) {
		//        if (s.width == r.width && s.height == r.height) {
		//            availableRes.Add(r);
		//        }
		//    }
		//}
		//if (availableRes.Count == 0) {
		//    availableRes = supportedResolutions.ToList();
		//}
		availableRes = supportedResolutions.ToList();
        availableRes.OrderByDescending(r => r.width);
        bgmSource = GetComponent<AudioSource>();

        InitializeSettings();
    }

    public static void InitializeSettings() {
        if (!PlayerPrefs.HasKey(SaveKeys.MasterVolume)) {
            PlayerPrefs.SetFloat(SaveKeys.MasterVolume, DEFAULT_MASTER_VOLUME);
        } else {
            MasterVolume = PlayerPrefs.GetFloat(SaveKeys.MasterVolume);
        }

        if (!PlayerPrefs.HasKey(SaveKeys.SFXVolume)) {
            PlayerPrefs.SetFloat(SaveKeys.SFXVolume, DEFAULT_SFX_VOLUME);
        } else {
            SFXVolume = PlayerPrefs.GetFloat(SaveKeys.SFXVolume);
        }

        if (!PlayerPrefs.HasKey(SaveKeys.MusicVolume)) {
            PlayerPrefs.SetFloat(SaveKeys.MusicVolume, DEFAULT_MUSIC_VOLUME);
        } else {
            MusicVolume = PlayerPrefs.GetFloat(SaveKeys.MusicVolume);
        }

        if (!PlayerPrefs.HasKey(SaveKeys.AnimationsEnabled)) {
            PlayerPrefs.SetString(SaveKeys.AnimationsEnabled, DEFAULT_ANIMATIONS.ToString());
        } else {
            AnimationsEnabled = PlayerPrefs.GetString(SaveKeys.AnimationsEnabled) == "True";
        }

        if (!PlayerPrefs.HasKey(SaveKeys.ScreenResolution)) {
            PlayerPrefs.SetInt(SaveKeys.ScreenResolution, DEFAULT_RESOLUTION);
        } else {
            ScreenResolution = PlayerPrefs.GetInt(SaveKeys.ScreenResolution);
        }

        if (!PlayerPrefs.HasKey(SaveKeys.FullscreenEnabled)) {
            PlayerPrefs.SetString(SaveKeys.FullscreenEnabled, DEFAULT_FULLSCREEN.ToString());
        } else {
            Fullscreen = PlayerPrefs.GetString(SaveKeys.FullscreenEnabled) == "True";
        }
    }

	public static void RestoreDefaults() {
		PlayerPrefs.SetFloat(SaveKeys.MasterVolume, DEFAULT_MASTER_VOLUME);
		PlayerPrefs.SetFloat(SaveKeys.SFXVolume, DEFAULT_SFX_VOLUME);
		PlayerPrefs.SetFloat(SaveKeys.MusicVolume, DEFAULT_MUSIC_VOLUME);
		PlayerPrefs.SetString(SaveKeys.AnimationsEnabled, DEFAULT_ANIMATIONS.ToString());
		PlayerPrefs.SetInt(SaveKeys.ScreenResolution, DEFAULT_RESOLUTION);
		PlayerPrefs.SetString(SaveKeys.FullscreenEnabled, DEFAULT_FULLSCREEN.ToString());
	}

    private void OnApplicationQuit() {
        //These will also be saved whenever they're changed, but I figured let's check just in case

        PlayerPrefs.SetFloat(SaveKeys.MasterVolume, MasterVolume);
        PlayerPrefs.SetFloat(SaveKeys.SFXVolume, SFXVolume);
        PlayerPrefs.SetFloat(SaveKeys.MusicVolume, MusicVolume);
        PlayerPrefs.SetString(SaveKeys.AnimationsEnabled, AnimationsEnabled.ToString());
		PlayerPrefs.SetInt(SaveKeys.ScreenResolution, ScreenResolution);
		PlayerPrefs.SetString(SaveKeys.FullscreenEnabled, Fullscreen.ToString());
    }

    public static float MasterVolume {
        get { return masterVolume; }
        set {
            masterVolume = value;
            PlayerPrefs.SetFloat(SaveKeys.MasterVolume, value);
            bgmSource.volume = masterVolume * musicVolume;
        }
    }

    public static float SFXVolume {
        get { return sfxVolume; }
        set {
            sfxVolume = value;
            PlayerPrefs.SetFloat(SaveKeys.SFXVolume, value);
        }
    }

    public static float MusicVolume {
        get { return musicVolume; }
        set {
            musicVolume = value;
            PlayerPrefs.SetFloat(SaveKeys.MusicVolume, value);
            bgmSource.volume = masterVolume * musicVolume;
        }
    }

    public static int ScreenResolution {
        get { return screenResolution; }
        set {
            screenResolution = Mathf.Clamp(value, 0, availableRes.Count - 1);
            Screen.SetResolution(availableRes[screenResolution].width, availableRes[screenResolution].height, fullscreen);
            MenuButtons.ResolutionChanged?.Invoke();
            PlayerPrefs.SetInt(SaveKeys.ScreenResolution, value);
        }
    }

    public static IEnumerable<Resolution> AvailableResolutions { get { return availableRes; } }

    public static bool AnimationsEnabled {
        get { return true; } //removing feature but leaving the skeleton in just in case
        set {
            animationsEnabled = value;
            PlayerPrefs.SetString(SaveKeys.AnimationsEnabled, value.ToString());
        }
    }

    public static bool Fullscreen {
        get { return fullscreen; }
        set {
            fullscreen = value;
            PlayerPrefs.SetString(SaveKeys.FullscreenEnabled, value.ToString());
            Screen.fullScreen = value;
            MenuButtons.ResolutionChanged?.Invoke();
            Screen.fullScreenMode = value ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        }
    }
}
