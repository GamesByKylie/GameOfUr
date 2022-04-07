using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameInfoScreen : MonoBehaviour
{
	public enum MiniGame { TavernaStart, TavernaPause, TavernaEnd }

	public TextMeshProUGUI titleText;
	public TextMeshProUGUI subtitleText;
	public TextMeshProUGUI contentText;
	public Scrollbar vertScroll;
	public Image iconIMG;
	public GameObject[] buttons;

	/// <summary>
	/// Displays the information on the InfoScreen object
	/// </summary>
	/// <param name="title">Title of this screen</param>
	/// <param name="subtitle">Subtitle of this screen</param>
	/// <param name="content">Text to be displayed on this screen</param>
	/// <param name="icon">Icon on this screen</param>
	/// <param name="type">Type of buttons to show</param>
	public void DisplayText(string title, string subtitle, string content, Sprite icon, MiniGame type) 
	{
		StartCoroutine(TextDisplay(title, subtitle, content, icon, type));
	}

	/// <summary>
	/// Actually does the information display and sets the scrollbar back to the top
	/// </summary>
	/// <param name="title"></param>
	/// <param name="subtitle"></param>
	/// <param name="content"></param>
	/// <param name="icon"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	private IEnumerator TextDisplay(string title, string subtitle, string content, Sprite icon, MiniGame type) 
	{
		titleText.text = title;
		subtitleText.text = subtitle;
		contentText.text = content;
		iconIMG.sprite = icon;
		ChangeButtons(type);
		//You need to wait a frame for the text object to understand its new size so moving the scroll to the top actually works
		yield return null;
		vertScroll.value = 1;
	}

	/// <summary>
	/// Switches what set of buttons is being displayed
	/// </summary>
	/// <param name="type"></param>
	private void ChangeButtons(MiniGame type) 
	{
		for (int i = 0; i < buttons.Length; i++) 
		{
			//Buttons need to be in the same order as the enum because we're casting it here
			if (i == (int)type) 
			{
				buttons[i].SetActive(true);
			}
			else 
			{
				buttons[i].SetActive(false);
			}
		}
	}

	public void AddToText(string add) {
		contentText.text += add;
	}

	public void CloseDialog() 
	{
		gameObject.SetActive(false);
	}
}
