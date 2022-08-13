using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharacter", menuName = "ScriptableObjects/Playable Character", order = 0)]
public class PlayableCharacter : ScriptableObject
{
    public Sprite characterIcon;
    public string characterName;
    public string subtitle;
}
