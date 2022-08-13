using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	private void Awake()
	{
		if (!PlayerPrefs.HasKey(SaveKeys.ShortestEasy)) {
			SaveValue(SaveKeys.ShortestEasy, int.MaxValue);
		}
		if (!PlayerPrefs.HasKey(SaveKeys.ShortestMedium)) {
			SaveValue(SaveKeys.ShortestMedium, int.MaxValue);
		}
		if (!PlayerPrefs.HasKey(SaveKeys.ShortestHard)) {
			SaveValue(SaveKeys.ShortestHard, int.MaxValue);
		}
	}

	public static void RestoreDefaults()
	{
		SaveValue(SaveKeys.ShortestEasy, int.MaxValue);
		SaveValue(SaveKeys.ShortestMedium, int.MaxValue);
		SaveValue(SaveKeys.ShortestHard, int.MaxValue);
	}

	public static void SaveValue(string key, int value) {
		PlayerPrefs.SetInt(key, value);
	}

	public static int LoadValue(string key) {
		if (PlayerPrefs.HasKey(key)) {
			return PlayerPrefs.GetInt(key);
		} else {
			return -1;
		}
	}

	public static void IncrementValue(string key)
	{
		PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key) + 1);
	}
}
