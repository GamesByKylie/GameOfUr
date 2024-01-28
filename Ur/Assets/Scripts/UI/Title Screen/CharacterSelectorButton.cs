using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectorButton : MonoBehaviour
{
    public PlayableCharacter character;
    public Image portrait;
	public Image portraitHolder;
    public Text nameText;
    public Text subtitleText;

	private Toggle tog;
	
    private void Start() {
        portrait.sprite = character.characterIcon;
		portraitHolder.sprite = character.characterIcon;
        nameText.text = character.characterName;
        subtitleText.text = character.subtitle;
		tog = GetComponent<Toggle>();
    }

    public void SetCharacter(bool toggle) {
		if (toggle) {
			if (tog.group.allowSwitchOff) {
				tog.group.allowSwitchOff = false;
			}
			if(GameManager.SelectedCharacter != character) {
				GameManager.SelectedCharacter = character;
				MenuButtons.PlayerSelection?.Invoke();
				GameManager.PlayButtonClick();
			}
		}
    }
}
