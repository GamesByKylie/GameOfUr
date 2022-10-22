using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
	public void ButtonHover() {
		GameManager.PlayButtonHover();
	}

	public void ButtonClick() {
		GameManager.PlayButtonClick();
	}

	public void MenuOpen() {
		GameManager.PlayMenuOpen();
	}

	public void MenuClose() {
		GameManager.PlayMenuClosed();
	}

	public void GameStart() {
		GameManager.PlayGameStart();
	}
}
