using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class StatBlock
{
	public TextMeshProUGUI easyText;
	public TextMeshProUGUI mediumText;
	public TextMeshProUGUI hardText;
}

public class StatsScreen : MonoBehaviour
{
	public StatBlock totalGames;
	public StatBlock wins;
	public StatBlock losses;
	public StatBlock shortest;
	public StatBlock longest;

	private void OnEnable()
	{
		FillText(totalGames, SaveKeys.TotalGamesEasy, SaveKeys.TotalGamesMedium, SaveKeys.TotalGamesHard);
		FillText(wins, SaveKeys.WinsEasy, SaveKeys.WinsMedium, SaveKeys.WinsHard);
		FillText(losses, SaveKeys.LossesEasy, SaveKeys.LossesMedium, SaveKeys.LossesHard);
		FillText(shortest, SaveKeys.ShortestEasy, SaveKeys.ShortestMedium, SaveKeys.ShortestHard);
		FillText(longest, SaveKeys.LongestEasy, SaveKeys.LongestMedium, SaveKeys.LongestHard);
	}

	private void FillText(StatBlock block, string easyKey, string medKey, string hardKey)
	{
		int value = SaveManager.LoadValue(easyKey);
		block.easyText.text = value < 0 || value == int.MaxValue ? "-" : value.ToString();
		value = SaveManager.LoadValue(medKey);
		block.mediumText.text = value < 0 || value == int.MaxValue ? "-" : value.ToString();
		value = SaveManager.LoadValue(hardKey);
		block.hardText.text = value < 0 || value == int.MaxValue ? "-" : value.ToString();
	}
}
