using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRefresher : MonoBehaviour
{
	private void OnEnable() {
		Canvas.ForceUpdateCanvases();
	}
}
