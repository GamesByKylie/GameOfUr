using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelAnimator : MonoBehaviour
{
	public Animator anim;
	public Scrollbar scroll;

	public void EnableAnimation(bool enable) {
		StartCoroutine(ResetScroll());
		anim.SetBool("MenuActive", enable);

	}

	private IEnumerator ResetScroll() {
		yield return null;
		yield return null;
		if(scroll != null) {
			scroll.value = 1f;
		}
	}
}
