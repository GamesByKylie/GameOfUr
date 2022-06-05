using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class TutorialInfo
{
    [TextArea(3, 15)]
    public string text;
    public Vector2 anchorMin;
    public Vector2 anchorMax;
    public GameObject[] turnOnAtStart;
    public GameObject[] turnOffAtStart;
    public GameObject[] turnOnAtEnd;
    public GameObject[] turnOffAtEnd;
}
