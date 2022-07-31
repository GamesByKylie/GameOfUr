using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayBox : MonoBehaviour
{
    public Animator anim;
    public Vector2 textPadding;

    [SerializeField] private Text messageText = null;
    [SerializeField] private RectTransform rect = null;

    //Also going to want an appear/disappear anim so it's not just sudden

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    //private void OnValidate()
    //{
    //    messageHolder.sizeDelta = new Vector2(rect.sizeDelta.x - textPadding.x, rect.sizeDelta.y - textPadding.y);
    //}

    public void DisplayMessage(string message, Vector2 anchorMin, Vector2 anchorMax)
    {
        if (rect == null) {
            rect = GetComponent<RectTransform>();
        }
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = Vector2.zero;
        messageText.text = message;
        //SetPadding(anchorMin);
    }

    private void SetPadding(Vector2 size)
    {
        messageText.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x - textPadding.x, size.y - textPadding.y);
    }
}
