using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernaMiniGameDialog : MonoBehaviour
{
    public Text nameText;
    public Image portrait;

    public float displayTime = 5f;
	public GameObject textBackground;
	public Text dialog;
    
    protected Coroutine displayRoutine;

    protected virtual void Start() 
	{
        SetDisplayInformation();
        textBackground.SetActive(false);
	}

    protected virtual void SetDisplayInformation()
    {
        nameText.text = GameManager.SelectedCharacter.characterName;
        portrait.sprite = GameManager.SelectedCharacter.characterIcon;
    }

	/// <summary>
	/// Displays an insult
	/// </summary>
	public void DisplayInsult() {
        if (displayRoutine != null) {
            StopCoroutine(displayRoutine);
        }

		textBackground.SetActive(true);
		dialog.text = GameManager.UrInsults.RandomElement();
        displayRoutine = StartCoroutine(AutoCloseDialog());
	}

	public void DisplayFromList(List<string> barkList) {
        if (displayRoutine != null) {
            StopCoroutine(displayRoutine);
        }

		textBackground.SetActive(true);
		dialog.text = barkList.RandomElement();
        displayRoutine = StartCoroutine(AutoCloseDialog());
	}

	public void CloseDialog() {
		if(displayRoutine != null) {
	        StopCoroutine(displayRoutine);
		}
		textBackground.SetActive(false);
	}

	public virtual void ShowCharacterInfo() {
		if(displayRoutine != null) {
			StopCoroutine(displayRoutine);
		}

		textBackground.SetActive(true);
		dialog.text = GameManager.SelectedCharacter.characterInfo;
	}

    private IEnumerator AutoCloseDialog()
    {
        yield return new WaitForSeconds(displayTime);
        CloseDialog();
    }
}
