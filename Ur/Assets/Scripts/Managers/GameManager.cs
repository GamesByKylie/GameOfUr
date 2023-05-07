using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text loadingText;
	public AudioSource bgmSource;
	public AudioSource sfxSource;
    public bool goToMainMenu = false;

	[Header("Region")]
	public AudioClip buttonHoverSound;
	public AudioClip buttonClickSound;
	public AudioClip menuOpenSound;
	public AudioClip menuCloseSound;
	public AudioClip gameStartSound;

	private static Scene persistantScene;
    private static GameManager instance;

	public static List<CrewMember> MasterCrewList { get; set; }
	public static UrAIController.AIDifficulty SelectedDifficulty { get; set; }
	public static PlayableCharacter SelectedCharacter { get; set; }
	public static List<string> UrFlavor { get; private set; }
	public static List<string> UrInsults { get; private set; }
	public static List<string> UrWinText { get; private set; }
	public static List<string> UrLoseText { get; private set; }
	public static List<string> UrRosetteText { get; private set; }
	public static List<string> UrFlipText { get; private set; }
	public static List<string> UrCaptureText { get; private set; }
	public static List<string> UrMoveOnText { get; private set; }
	public static List<string> UrMoveOffText { get; private set; }

	void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            instance.SetLoadingText("");
            MasterCrewList = CSVLoader.LoadMasterCrewRoster();
            CSVLoader.LoadUrText();
            persistantScene = SceneManager.GetSceneByBuildIndex(0);
			AudioSettings.OnAudioConfigurationChanged += deviceWasChanged => {
				Debug.LogWarning($"Audio device change detected: {deviceWasChanged}");
				bgmSource.Pause();
				bgmSource.UnPause();
			};
            if (goToMainMenu) {
                LoadMainMenu();
            }
        }
    }

	public static void PlaySFX(AudioClip clip) {
		instance.sfxSource.Stop();
		instance.sfxSource.volume = SettingsManager.MasterVolume * SettingsManager.SFXVolume;
		instance.sfxSource.clip = clip;
		instance.sfxSource.Play();
	}

	public static void PlayButtonHover() {
		PlaySFX(instance.buttonHoverSound);
	}

	public static void PlayButtonClick() {
		PlaySFX(instance.buttonClickSound);
	}

	public static void PlayMenuOpen() {
		PlaySFX(instance.menuOpenSound);
	}

	public static void PlayMenuClosed() {
		PlaySFX(instance.menuCloseSound);
	}

	public static void PlayGameStart() {
		PlaySFX(instance.gameStartSound);
	}

	public static void SetTextLists(List<string> flavor, List<string> insults, List<string> win, List<string> lose, List<string> rosette, List<string> flip, List<string> capture,
		List<string> moveOn, List<string> moveOff) {
		UrFlavor = flavor;
		UrInsults = insults;
		UrWinText = win;
		UrLoseText = lose;
		UrRosetteText = rosette;
		UrFlipText = flip;
		UrCaptureText = capture;
		UrMoveOnText = moveOn;
		UrMoveOffText = moveOff;
	}

    //Since we're using Async scene loading, we need to do that through a coroutine
    //But it's going to be much, much more convenient if we can call these scene loaders through static methods
    //So we make this instance a static variable, then use that to call the coroutine
    //Since you're not allowed to start coroutines inside a static method usually

    //Instead of having the coroutine being public so it can be called from anywhere, I just made methods for each scene
    //There's so few scenes that this is entirely feasible for this project
    //Plus, having these as static methods means we won't have to use GameObject.FindWithTag to make this script a variable EVERYWHERE
    //(because this is in the master scene, we can't just assign it as a variable in the inspector)
    //We will still have to write new methods to call these from buttons, but that's much easier
    //I'm not even sure if you can start a coroutine through a button without another encapsulating normal method, I don't think you can
    public static void LoadMainMenu() {
        instance.StartCoroutine(instance.LoadScene(1));
    }

    public static void LoadGamePlay() {
        instance.StartCoroutine(instance.LoadScene(2));
    }

    private IEnumerator LoadScene(int index) {
        //If you're loading a scene from the pause menu, timeScale is 0, so we need to reset it
        //Most of this will still work, but not the artificially inflated loading
        Time.timeScale = 1;
        if (SceneManager.GetSceneByBuildIndex(index) == null) {
            Debug.Log($"Scene at index {index} is null");
        }

        if (SceneManager.GetActiveScene().Equals(persistantScene)) {
            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            Scene nextScene = SceneManager.GetSceneByBuildIndex(index);
            SceneManager.SetActiveScene(nextScene);
        } else {
            Scene currentScene = SceneManager.GetActiveScene();

            SceneManager.SetActiveScene(persistantScene);
            
            yield return SceneManager.UnloadSceneAsync(currentScene);

            //Going to add a bit of artificial load time in here so you can see the "loading" screen instead of it just flashing for an instant
            //We want to minimize flashing images, obviously, and this way it's also easier to tell that it's a loading screen
            //This will probably be replaced by something nicer looking later when I learn how to do that
            instance.SetLoadingText("Loading...");
            yield return new WaitForSeconds(0.25f);
            instance.SetLoadingText("");

            yield return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);


            //This needs to be down here, at the end, instead of up where we set currentScene
            //I'll be honest - I'm not entirely sure why, but the game crashes otherwise
            //It's got to have something to do with the order scenes are loaded in, but I'm not sure
            Scene nextScene = SceneManager.GetSceneByBuildIndex(index);

            SceneManager.SetActiveScene(nextScene);
        }
    }

    private void SetLoadingText(string text) {
        if (loadingText != null) {
            loadingText.text = text;
        }
    }
}
