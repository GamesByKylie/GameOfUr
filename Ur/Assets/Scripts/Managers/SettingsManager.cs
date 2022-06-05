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
        new Resolution{ width = 1920, height = 1080 },
        new Resolution{ width = 2560, height = 1440 },
        new Resolution{ width = 1366, height = 768 },
        new Resolution{ width = 3840, height = 2160 },
        new Resolution{ width = 1600, height = 900 },
        new Resolution{ width = 3440, height = 1440 },
        new Resolution{ width = 1360, height = 768 },
        new Resolution{ width = 2560, height = 1600 },
        new Resolution{ width = 2560, height = 1080 },
        new Resolution{ width = 1920, height = 1200 }
        };
    private static List<Resolution> availableRes = new List<Resolution>();

    private void Awake()
    {
        List<Resolution> potentialRes = Screen.resolutions.Select(r => new Resolution { width = r.width, height = r.height }).Distinct().ToList();
        foreach (var r in potentialRes)
        {
            foreach (var s in supportedResolutions)
            {
                if (s.width == r.width && s.height == r.height)
                {
                    availableRes.Add(r);
                }
            }
        }
        if (availableRes.Count == 0)
        {
            availableRes = supportedResolutions.ToList();
        }
        availableRes.OrderByDescending(r => r.width);
        bgmSource = GetComponent<AudioSource>();

        InitializeSettings();
    }

    public static void InitializeSettings()
    {
        if (!PlayerPrefs.HasKey("master_volume"))
        {
            PlayerPrefs.SetFloat("master_volume", DEFAULT_MASTER_VOLUME);
        }
        else
        {
            MasterVolume = PlayerPrefs.GetFloat("master_volume");
        }

        if (!PlayerPrefs.HasKey("sfx_volume"))
        {
            PlayerPrefs.SetFloat("sfx_volume", DEFAULT_SFX_VOLUME);
        }
        else
        {
            SFXVolume = PlayerPrefs.GetFloat("sfx_volume");
        }

        if (!PlayerPrefs.HasKey("music_volume"))
        {
            PlayerPrefs.SetFloat("music_volume", DEFAULT_MUSIC_VOLUME);
        }
        else
        {
            MusicVolume = PlayerPrefs.GetFloat("music_volume");
        }

        if (!PlayerPrefs.HasKey("animations_enabled"))
        {
            PlayerPrefs.SetString("animations_enabled", DEFAULT_ANIMATIONS.ToString());
        }
        else
        {
            AnimationsEnabled = PlayerPrefs.GetString("animations_enabled") == "True";
        }

        if (!PlayerPrefs.HasKey("screen_resolution"))
        {
            PlayerPrefs.SetInt("screen_resolution", DEFAULT_RESOLUTION);
        }
        else
        {
            ScreenResolution = PlayerPrefs.GetInt("screen_resolution");
        }

        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetString("fullscreen", DEFAULT_FULLSCREEN.ToString());
        }
        else
        {
            Fullscreen = PlayerPrefs.GetString("fullscreen") == "True";
        }
    }

    private void OnApplicationQuit()
    {
        //These will also be saved whenever they're changed, but I figured let's check just in case

        PlayerPrefs.SetFloat("master_volume", MasterVolume);
        PlayerPrefs.SetFloat("sfx_volume", SFXVolume);
        PlayerPrefs.SetFloat("music_volume", MusicVolume);
        PlayerPrefs.SetString("animations_enabled", AnimationsEnabled.ToString());
        PlayerPrefs.SetString("fullscreen", Fullscreen.ToString());
    }

    public static float MasterVolume
    {
        get { return masterVolume; }
        set
        {
            masterVolume = value;
            PlayerPrefs.SetFloat("master_volume", value);
            bgmSource.volume = masterVolume * musicVolume;
        }
    }

    public static float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            PlayerPrefs.SetFloat("sfx_volume", value);
        }
    }

    public static float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("music_volume", value);
            bgmSource.volume = masterVolume * musicVolume;
        }
    }

    public static int ScreenResolution
    {
        get
        {
            return screenResolution;
        }
        set
        {
            screenResolution = Mathf.Clamp(value, 0, availableRes.Count - 1);
            Debug.Log($"Trying to set resolution to number {value} (final value {screenResolution}");
            Screen.SetResolution(availableRes[screenResolution].width, availableRes[screenResolution].height, fullscreen);
            PlayerPrefs.SetInt("screen_resolution", value);
        }
    }

    public static IEnumerable<Resolution> AvailableResolutions
    {
        get
        {
            return availableRes;
        }
    }

    public static bool AnimationsEnabled
    {
        get { return animationsEnabled; }
        set
        {
            animationsEnabled = value;
            PlayerPrefs.SetString("animations_enabled", value.ToString());
        }
    }

    public static bool Fullscreen
    {
        get { return fullscreen; }
        set
        {
            fullscreen = value;
            PlayerPrefs.SetString("fullscreen", value.ToString());
            Screen.fullScreen = value;
            Screen.fullScreenMode = value ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        }
    }
}
