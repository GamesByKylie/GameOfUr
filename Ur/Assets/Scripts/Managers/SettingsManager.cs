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
    private static IEnumerable<Resolution> availableRes;

    private void Awake()
    {
        availableRes = Screen.resolutions.Select(r => new Resolution { width = r.width, height = r.height }).Distinct();
        availableRes = availableRes.Reverse();
        bgmSource = GetComponent<AudioSource>();

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
            screenResolution = value;
            Screen.SetResolution(availableRes.ElementAt(value).width, availableRes.ElementAt(value).height, fullscreen);
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
