using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExplanation : MonoBehaviour
{
	public Image explanation;
	public TMPro.TextMeshProUGUI text;
	public float edging = 25;
	public float maxWidth;

	private RectTransform explanationRect;
	private bool sizeSet = false;
	private Color explanationCol = Color.white;
	private Color textCol = Color.white;

	private void OnEnable() 
	{
		//Sets private variables
		if (explanationRect == null) 
		{
			explanationRect = explanation.GetComponent<RectTransform>();
			explanationCol = explanation.color;
			textCol = text.color;
		}

		//If the text is turned on to start, hide it
		//The text will need to be turned on in the prefab or else the sizing won't work right
		HideText();
	}

	/// <summary>
	/// Shows the explanation image and text
	/// </summary>
	public void DisplayText() 
	{
		if (!sizeSet) {
			//If the size hasn't been set properly, set it and let that method show it
			StartCoroutine(SetSize());
		}
		else {
			//Otherwise, you don't need to waste time sizing it to the same size, so just enable it
			explanation.gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// Hides the explanation image and text
	/// </summary>
	public void HideText() 
	{
		explanation.gameObject.SetActive(false);
	}

	/// <summary>
	/// Sets the text to be shown as the explanation
	/// </summary>
	/// <param name="toDisplay">Explanation text</param>
	public void SetExplanationText(string toDisplay) 
	{
		text.text = toDisplay;
		sizeSet = false;

		if (explanationRect == null) {
			//If the rect transform isn't set, it'll cause problems, so set it here
			explanationRect = explanation.GetComponent<RectTransform>();
		}
	}

	/// <summary>
	/// Sets the size of the explanation image to match the explanation text
	/// </summary>
	/// <returns></returns>
	private IEnumerator SetSize() 
	{
		explanation.gameObject.SetActive(true);
		//You can't set the size properly on a disabled object, but since this takes a frame to kick in, if it's visible you'll see it small for a split second
		//The solution is to keep it enabled but hidden so you can size it without it being visible
		InvisibleExplanation();
		//Wait for the text to register how big it is now so text.preferredWidth works
		yield return null;

		float boxWidth = Mathf.Min(text.preferredWidth, maxWidth) + (2 * edging);
		//You're going to think that you can just set explanationRect.rect's width directly, and that doesn't work
		//If you're trying to directly change the width/height, it has to be with sizeDelta for some reason
		explanationRect.sizeDelta = new Vector2(boxWidth, text.fontSize);

		//Now that the width is set, you can set the height to match the overflow
		float boxHeight = text.preferredHeight + (2 * edging);
		explanationRect.sizeDelta = new Vector2(boxWidth, boxHeight);

		//Turn it back to visible
		VisibleExplanation();
		sizeSet = true;
	}

	/// <summary>
	/// Turns the explanation image and text invisible while still keeping them enabled
	/// </summary>
	private void InvisibleExplanation() 
	{
		if (explanationCol == Color.white && textCol == Color.white) 
		{
			//If the colors haven't already been set to the actual start colors, set them
			explanationCol = explanation.color;
			textCol = text.color;
		}

		//Set both image and text to alpha 0
		explanation.color = Color.clear;
		text.color = Color.clear;
	}

	/// <summary>
	/// Turns the invisible image and text back to their original colors
	/// </summary>
	private void VisibleExplanation() 
	{
		//Set image and text back to their original colors
		explanation.color = explanationCol;
		text.color = textCol;
	}
}
