using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RectTransformRefresher : MonoBehaviour
{
	private RectTransform rt;

	private void Awake() {
		rt = GetComponent<RectTransform>();
	}

	private void OnEnable() {
		Refresh();
		MenuButtons.ResolutionChanged += Refresh;
	}

	private void OnDisable() {
		MenuButtons.ResolutionChanged -= Refresh;
	}

	private void Refresh() {
		LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
	}
}
