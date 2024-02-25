using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PositionedRectTransform
{
	public RectTransform rect;
	public Vector2 normalPos;
	public Vector2 widePos;
}

public class ResolutionRectPositioner : MonoBehaviour
{
	public PositionedRectTransform[] positionedObjs;
	public float wideAspect = 2;

    private Vector2 screenSize;

	private void OnEnable() {
		MenuButtons.ResolutionChanged += PositionRects;
	}

	private void OnDisable() {
		MenuButtons.ResolutionChanged -= PositionRects;
	}

	private void PositionRects() {
		StartCoroutine(DoPositionRects());
	}

	private IEnumerator DoPositionRects() {
		yield return null;

		float aspect = (Screen.width * 1.0f) / Screen.height;

		if (aspect >= wideAspect) {
			foreach(var prt in positionedObjs) {
				prt.rect.anchoredPosition = prt.widePos;
			}
		} else {
			foreach(var prt in positionedObjs) {
				prt.rect.anchoredPosition = prt.normalPos;
			}
		}

		Canvas.ForceUpdateCanvases();
	}


}
