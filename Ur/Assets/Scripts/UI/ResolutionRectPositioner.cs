using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PositionedRectTransform
{
	public RectTransform rect;
	public Vector2 normalAnchor;
	public Vector2 wideAnchor;
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
				prt.rect.anchorMax = prt.wideAnchor;
				prt.rect.anchorMin = prt.wideAnchor;

				prt.rect.anchoredPosition = Vector2.zero;
			}
		} else {
			foreach(var prt in positionedObjs) {
				prt.rect.anchorMax = prt.normalAnchor;
				prt.rect.anchorMin = prt.normalAnchor;

				prt.rect.anchoredPosition = Vector2.zero;
			}
		}
	}


}
