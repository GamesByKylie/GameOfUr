using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectorButton : MonoBehaviour
{
    public PlayableCharacter character;
    public Image portrait;
    public Text nameText;
    public Text subtitleText;

    private void Start()
    {
        portrait.sprite = character.characterIcon;
        nameText.text = character.characterName;
        subtitleText.text = character.subtitle;
    }

    public void SetCharacter()
    {
        GameManager.SelectedCharacter = character;
    }
}
