using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrScrollTex : MonoBehaviour
{
	public float xSpeed;
	public float ySpeed;

	private Renderer rend;

	private void Start() {
		rend = GetComponent<Renderer>();
	}

	private void Update() {
		float updatedX = Time.time * xSpeed;
		float updatedY = Time.time * ySpeed;

		rend.material.mainTextureOffset = new Vector2(updatedX, updatedY);
	}
}
