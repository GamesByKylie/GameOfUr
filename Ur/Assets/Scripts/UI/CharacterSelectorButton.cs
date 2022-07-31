using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectorButton : MonoBehaviour
{
    public PlayableCharacter character;

    public void SetCharacter()
    {
        GameManager.SelectedCharacter = character;
    }
}
