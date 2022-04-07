using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    private const float DEFAULT_MASTER_VOLUME = 1.0f;
    private const float DEFAULT_SFX_VOLUME = 0.75f;
    private const float DEFAULT_MUSIC_VOLUME = 0.5f;

    private const bool DEFAULT_ANIMATIONS = true;

    private static float masterVolume;
    private static float sfxVolume;
    private static float musicVolume;
    private static bool animationsEnabled;

    private static AudioSource bgmSource;

    private void Awake()
    {
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
            AnimationsEnabled = PlayerPrefs.GetString("animations_enabled") == "true";
        }
    }

    private void OnApplicationQuit()
    {
        //These will also be saved whenever they're changed, but I figured let's check just in case

        PlayerPrefs.SetFloat("master_volume", MasterVolume);
        PlayerPrefs.SetFloat("sfx_volume", SFXVolume);
        PlayerPrefs.SetFloat("music_volume", MusicVolume);
        PlayerPrefs.SetString("animations_enabled", AnimationsEnabled.ToString());
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

    public static bool AnimationsEnabled
    {
        get { return animationsEnabled; }
        set
        {
            animationsEnabled = value;
            PlayerPrefs.SetString("animations_enabled", value.ToString());
        }
    }

    public static Vector2 ScreenResolution
    {
        get; set;
    }
}
