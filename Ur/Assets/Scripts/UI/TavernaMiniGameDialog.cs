using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernaMiniGameDialog : MonoBehaviour
{
	public GameObject textBackground;
	public Text dialog;

    protected GameManager gm;

    private void Start() 
	{
        gm = GameObject.FindWithTag("Master").GetComponent<GameManager>();
        textBackground.SetActive(false);
	}

	/// <summary>
	/// Displays an insult
	/// </summary>
	public void DisplayInsult() {
		//Time.timeScale = 0;
		textBackground.SetActive(true);
		dialog.text = gm.urInsults.RandomElement();
	}

	/// <summary>
	/// Displays a brag
	/// </summary>
	public void DisplayBragging() {
		//Time.timeScale = 0;
		textBackground.SetActive(true);
		//dialog.text = braggingTexts.RandomElement();
	}

	public void DisplayFromList(List<string> barkList) {
		//Time.timeScale = 0;
		textBackground.SetActive(true);
		//dialog.text = barkList.RandomElement();
	}

	public void CloseDialog() {
		//Time.timeScale = 1;
		textBackground.SetActive(false);
	}
}
