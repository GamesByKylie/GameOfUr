using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrPlayerPiece : UrPiece
{
    public static float endTurnPause = 0.75f;

    private LayerMask mask;

	private bool selected = false;
	private bool showingBark = false;

	private void Start() 
	{
		AssignVariables();
        mask = LayerMask.GetMask("GameSquare");
		//mask = ~0;
	}

	private void Update() 
	{
		if (selected) 
		{
			RaycastHit hit;
			Ray ray = urGC.mainCam.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit, 300f, mask.value, QueryTriggerInteraction.Collide)) 
			{
				UrGameTile ugt = hit.collider.GetComponent<UrGameTile>();
				if (ugt != null && validMoves.Contains(ugt)) 
				{
					transform.position = ugt.transform.position;

					//We have to do all this so we don't accidentally try to send the piece to an earlier index, which would break a lot of stuff
					List<int> allPossibleIndices = new List<int>();
					for (int i = 0; i < urGC.playerBoardPositions.Count; i++) 
					{
						if (urGC.playerBoardPositions[i].Equals(ugt)) {
							allPossibleIndices.Add(i);
						}
					}

					//We're going to hard-code this as always only having 2 potential tiles, since that's the way the game is set up
					//Honestly, I probably should come up with a better way, but I can't think of it right now
					if (allPossibleIndices.Count > 1) 
					{
						if (boardIndex > allPossibleIndices[0]) 
						{
							potentialIndex = allPossibleIndices[1];
						}
						else 
						{
							potentialIndex = allPossibleIndices[0];
						}
					}
					else 
					{
						potentialIndex = allPossibleIndices[0];
					}
					
					ClearPossiblePath();
					ShowPossiblePath(urGC.playerBoardPositions, boardIndex, potentialIndex);
				}
				else 
				{
					//The only thing tagged GameSquare other than board squares is the plane that lets you drag off the board
					//We do this because it feels weird to mouse off your opponent's side of the board but have your piece snap back to your side
					//And of course, we check if the last position actually was off the board, otherwise you shouldn't be able to
					if (boardIndex == -1) 
					{
						transform.position = startPos;
						potentialIndex = -1;
					}
				}
			}
		}
	}

	private void OnMouseDown() 
	{
		if (urGC.AllowPlayerMove && enabled && highlight.activeSelf) 
		{
			selected = true;
			urGC.UnhighlightPieces();
			SpawnGhostInPlace();
			potentialIndex = boardIndex;
			validMoves = PopulateValidMovesList(urGC.playerBoardPositions, true);
			urGC.UnhighlightBoard();
		
			foreach (UrGameTile tile in validMoves) 
			{
				tile.ShowHighlight(true, true);
			}
		}
	}

	private void OnMouseUp() 
	{
		if (urGC.AllowPlayerMove && enabled && selected) 
		{
			selected = false;
			DestroyGhost();
			urGC.UnhighlightBoard();
			ClearPossiblePath();

			//If you moved, check for flip or piece off, then pass the turn
			if (boardIndex != potentialIndex) 
			{
				showingBark = false;
				urGC.PlayMoveSound();

				if (boardIndex != -1) {
					urGC.playerBoardPositions[boardIndex].ClearOccupied();
				}
				else {
					//if (!showingBark) {
					//	urGC.TriggerBark(true, urGC.gm.urMoveOnText);
					//	showingBark = true;
					//}

				}
				
				//The "bridge" you go back along starts at index 16 for both player and enemy, so I'm hard-coding it in
				//Again, this is a bad practice, but it shouldn't change so it's probably fine
				if (boardIndex < 16 && potentialIndex >= 16) 
				{
					FlipPiece();
					//if (!showingBark) {
					//	urGC.TriggerBark(true, urGC.gm.urFlipText);
					//	showingBark = true;
					//}

				}

				boardIndex = potentialIndex;
				//Capture
				bool captureThisTurn = false;
				if (urGC.playerBoardPositions[boardIndex].OppositeOccupyingPiece(true)) 
				{
					captureThisTurn = true;
					urGC.playerBoardPositions[boardIndex].RemoveCurrentFromBoard();
					urGC.ShowAlertText("Captured the enemy!");
					//if (!showingBark) {
					//	urGC.TriggerBark(true, urGC.gm.urCaptureText);
					//	showingBark = true;
					//}

					urGC.PlaySoundFX(UrGameController.SoundTrigger.Capture, true);
				}

				//We check this now because we won't need to do any more processing if you're moving off the board
				//We especially don't want to set the end space to occupied!
				if (boardIndex == urGC.playerBoardPositions.Count - 1) 
				{
					//urGC.TriggerBark(true, urGC.gm.urMoveOffText, true);
					urGC.PointScored(true, this);
					urGC.PlaySoundFX(UrGameController.SoundTrigger.OffBoard, true);
				}
				else {
					urGC.playerBoardPositions[boardIndex].SetOccupied(this);
				}
				
				//If it's a rosette, "switch" to player turn
				if (urGC.playerBoardPositions[boardIndex].isRosette) 
				{
					if (captureThisTurn) {
						urGC.ShowAlertText("Captured the enemy! And Roll Again!");
					}
					else {
						urGC.ShowAlertText("Roll Again");
					}

					if (!showingBark) {
						//urGC.TriggerBark(true, urGC.gm.urRosetteText);
						showingBark = true;
					}

					urGC.PlaySoundFX(UrGameController.SoundTrigger.Rosette, true);
					urGC.SwitchTurn(true, false);
				}
				else 
				{
                    if (SettingsManager.AnimationsEnabled)
                    {
                        StartCoroutine(urGC.WaitToSwitchTurn(false, true, endTurnPause));
                    }
                    else
                    {
                        urGC.SwitchTurn(false, true);
                    }
				}

				//No clue why I would need to check this, but I got an error once out of nowhere!
				if (validMoves != null) 
				{
					validMoves.Clear();
				}
			}
			//If you didn't move, the highlights need to come back
			else 
			{
				urGC.CanPlayerMove(true);
			}
		}
	}	
}
