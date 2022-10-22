using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayBox : MenuPanelAnimator
{
    public Vector2 textPadding;

    [SerializeField] private Text messageText = null;
    [SerializeField] private RectTransform rect = null;

    //Also going to want an appear/disappear anim so it's not just sudden

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

	public void DisplayMessage(string message, Vector2 anchorMin, Vector2 anchorMax) {
        if (rect == null) {
            rect = GetComponent<RectTransform>();
        }
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = Vector2.zero;
        messageText.text = message;
    }
}
