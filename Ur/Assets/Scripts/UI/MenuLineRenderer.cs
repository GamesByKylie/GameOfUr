using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLineRenderer : MonoBehaviour
{
    public RectTransform line;
    public RectTransform pointAt;
    public float extraLength;

    private Vector2 screenSize;

    private void Start()
    {
        ResizeLine();
    }

    private void Update()
    {
        if (Screen.width != screenSize.x || Screen.height != screenSize.y)
        {
            ResizeLine();
        }
    }

    private void LateUpdate()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    public void ResizeLine()
    {
        Vector3 dir = pointAt.position - line.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


}
