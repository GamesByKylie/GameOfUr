using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMProHyperlink : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int test1 = 0;
    public int test2 = 1;

    public Color32 linkHoverColor;
    public Color32 baseColor;

    private bool isHovering = false;
    private TMP_Text text;
    Vector2Int linkStats = new Vector2Int(-1, -1);

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    //Credit to https://www.feelouttheform.net/unity3d-links-textmeshpro/
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer clicked");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo link = text.textInfo.linkInfo[linkIndex];
            Application.OpenURL(link.GetLinkID());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    //Credit to https://forum.unity.com/threads/text-mesh-pro-highlight-all-words-in-a-link.810882/
    private void LateUpdate()
    {
        if (isHovering)
        {
            if (linkStats.x != -1 && linkStats.y != -1)
            {
                for (int i = linkStats.x; i < linkStats.y; i++)
                {
                    if (text.textInfo.characterInfo[i].character != ' ')
                    {
                        ChangeColor(i, baseColor);
                    }
                }
            }

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);

            if (linkIndex != -1)
            {
                linkStats.x = text.textInfo.linkInfo[linkIndex].linkTextfirstCharacterIndex;
                linkStats.y = linkStats.x + text.textInfo.linkInfo[linkIndex].linkTextLength;

                for (int i = linkStats.x; i < linkStats.y; i++)
                {
                    if (i < text.textInfo.characterCount)
                    {
                        if (text.textInfo.characterInfo[i].character != ' ')
                        {
                            Debug.Log($"Changing color at {i}: {text.textInfo.characterInfo[i].character}");
                            ChangeColor(i, linkHoverColor);
                        }
                    }
                }
                text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
        }
    }

    private void ChangeColor(int index, Color32 col)
    {
        int meshIndex = text.textInfo.characterInfo[index].materialReferenceIndex;
        int vertexIndex = text.textInfo.characterInfo[index].vertexIndex;

        Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
        vertexColors[vertexIndex + 0] = col;
        vertexColors[vertexIndex + 1] = col;
        vertexColors[vertexIndex + 2] = col;
        vertexColors[vertexIndex + 3] = col;
    }

}
