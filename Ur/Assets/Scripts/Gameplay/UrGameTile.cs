//David Herrod
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UrGameTile : MonoBehaviour
{
	public bool isRosette = false;
	public Image highlight;
    public ParticleSystem ps;
	public Color playerHighlightColor;
	public Color enemyHighlightColor;

	private bool occupied = false;
	private UrPiece currentPiece = null;
	private UrGameController urGC;
    private ParticleSystem.MainModule psMain;
	

	private void Start() {
		urGC = GameObject.FindWithTag("GameController").GetComponent<UrGameController>();
        psMain = ps.main;
	}

	public void ShowHighlight(bool toggle, bool isPlayer = true) 
	{
		if (toggle) 
		{
			if (isPlayer) 
			{
				highlight.color = playerHighlightColor;
                psMain.startColor = playerHighlightColor;
			}
			else {
				highlight.color = enemyHighlightColor;
                psMain.startColor = enemyHighlightColor;
			}
		}

		if (highlight != null) 
		{
			highlight.gameObject.SetActive(toggle);
		}
	}

	public void SetOccupied(UrPiece p) 
	{
		currentPiece = p;
		occupied = true;
	}

	public void ClearOccupied() 
	{
		occupied = false;
		currentPiece = null;
	}
	
	public void RemoveCurrentFromBoard() 
	{
		if (currentPiece != null) 
		{
			currentPiece.RemovePieceFromBoard();
			currentPiece = null;
			occupied = false;
		}
	}

	/// <summary>
	/// Returns true if the piece on the occupying square is not controlled by the player indicated by isPlayer
	/// </summary>
	/// <param name="isPlayer"></param>
	/// <returns></returns>
	public bool OppositeOccupyingPiece(bool isPlayer) 
	{
		if (currentPiece == null) 
		{
			return false;
		}
		return isPlayer ? currentPiece.CompareTag(urGC.enemyTag) : currentPiece.CompareTag(urGC.playerTag);
	}

	public bool Occupied 
	{
		get 
		{
			return occupied;
		}
	}
}
