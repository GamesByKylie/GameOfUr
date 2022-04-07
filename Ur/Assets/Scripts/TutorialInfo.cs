using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class TutorialInfo
{
    [TextArea(3, 15)]
    public string text;
    public Vector2 position;
    public Vector2 scale;
    public GameObject[] turnOnAtStart;
    public GameObject[] turnOffAtStart;
    public GameObject[] turnOnAtEnd;
    public GameObject[] turnOffAtEnd;
}
