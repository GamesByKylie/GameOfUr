using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMatchScale : MonoBehaviour
{
    public ParticleSystem ps;
    public RectTransform match;

    private void OnEnable()
    {
        ParticleSystem.MainModule psMain = ps.main;
        float aspect = Screen.width * 1f / Screen.height;
        psMain.startSizeX = match.rect.width;
        psMain.startSizeY = match.rect.height;
    }
}
