using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernaMiniGameDialog : MonoBehaviour
{
    public Text nameText;
    public Image portrait;
	public Animator anim;
	public GameObject highlight;

    public float displayTime = 5f;
	public GameObject textBackground;
	public Text dialog;
	public Scrollbar dialogScroll;
    
    protected Coroutine displayRoutine;

    protected virtual void Start() 
	{
        SetDisplayInformation();
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

		ShowText(GameManager.UrInsults.RandomElement());
        displayRoutine = StartCoroutine(AutoCloseDialog());
	}

	public void DisplayFromList(List<string> barkList) {
        if (displayRoutine != null) {
            StopCoroutine(displayRoutine);
        }

		ShowText(barkList.RandomElement());
        displayRoutine = StartCoroutine(AutoCloseDialog());
	}

	public void CloseDialog() {
		if(displayRoutine != null) {
	        StopCoroutine(displayRoutine);
		}
		anim.SetBool("SpeechBubbleActive", false);
	}

	public virtual void ShowCharacterInfo() {
		if(displayRoutine != null) {
			StopCoroutine(displayRoutine);
		}

		ShowText(GameManager.SelectedCharacter.characterInfo);
	}

    private IEnumerator AutoCloseDialog() {
        yield return new WaitForSeconds(displayTime);
        CloseDialog();
    }

	public virtual void ShowText(string text) {
		anim.SetBool("SpeechBubbleActive", true);
		dialog.text = text;
		dialogScroll.value = 1;
	}

	public void EnableHighlight(bool enable) {
		highlight.SetActive(enable);
	}
}
