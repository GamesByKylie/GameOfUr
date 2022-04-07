using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayBox : MonoBehaviour
{
    public Animator anim;
    public Vector2 textPadding;

    [SerializeField] private Text messageText = null;
    [SerializeField] private RectTransform messageHolder = null;
    [SerializeField] private RectTransform rect = null;

    //Also going to want an appear/disappear anim so it's not just sudden

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnValidate()
    {
        messageHolder.sizeDelta = new Vector2(rect.sizeDelta.x - textPadding.x, rect.sizeDelta.y - textPadding.y);
    }

    public void DisplayMessage(string message, Vector2 size, Vector2 pos)
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;
        messageText.text = message;
        SetPadding(size);
    }

    private void SetPadding(Vector2 size)
    {
        messageText.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x - textPadding.x, size.y - textPadding.y);
    }
}
