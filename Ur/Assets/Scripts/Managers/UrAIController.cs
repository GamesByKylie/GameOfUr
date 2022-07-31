using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrAIController : MonoBehaviour
{
	public List<UrPiece> enemyPieces;

	public float midTurnPause = 0.5f;
	public float endTurnPause = 1f;
    public float noAnimMidPause = 0.25f;
    public float noAnimEndPause = 0.5f;

	private UrGameController urGC;
	private int currentRoll;
	private bool showingBark = false;

	private void Start() 
	{
		urGC = GetComponent<UrGameController>();
	}

	public void EnemyTurn() 
	{
		if (!urGC.IsGameOver) {
			currentRoll = urGC.GetDiceRoll();
		}
	}

	public IEnumerator DoEnemyTurn() 
	{
		if (currentRoll != 0) 
		{
			showingBark = false;
			//Picks out what pieces are valid
			List<UrPiece> movablePieces = new List<UrPiece>();
			for (int i = 0; i < enemyPieces.Count; i++) {
				if (enemyPieces[i].PopulateValidMovesList(urGC.enemyBoardPositions, false).Count != 0) {
					movablePieces.Add(enemyPieces[i]);
				}
			}

			bool redoTurn = false;
			if (movablePieces.Count > 0) 
			{
				//Pick what piece to move
				UrPiece pieceToMove = ChoosePieceToMove(movablePieces);
				pieceToMove.ShowHighlight(true);

				List<UrGameTile> validMoves = pieceToMove.PopulateValidMovesList(urGC.enemyBoardPositions, false);
				foreach (UrGameTile t in validMoves) {
					t.ShowHighlight(true, false);
				}
				//If we're moving it onto the board, it's only got one potential move. Otherwise, it has two - its current space and the next one
				int validMovePos = pieceToMove.BoardIndex == -1 ? 0 : 1;
				UrGameTile nextTile = validMoves[validMovePos];
                
    			yield return new WaitForSeconds(SettingsManager.AnimationsEnabled ? midTurnPause : noAnimMidPause);

				//Visually show the move
				urGC.UnhighlightPieces();
				pieceToMove.SpawnGhostInPlace();
				//Since we know that this space + currentRoll is a valid move, we don't have to check it again
				pieceToMove.ShowPossiblePath(urGC.enemyBoardPositions, pieceToMove.BoardIndex, pieceToMove.BoardIndex + currentRoll);
				pieceToMove.transform.position = nextTile.transform.position;

                yield return new WaitForSeconds(SettingsManager.AnimationsEnabled ? midTurnPause : noAnimMidPause);

                //Finalize the move

                urGC.PlayMoveSound();

				bool captureThisTurn = false;
				//Check for a capture
				if (nextTile.OppositeOccupyingPiece(false)) {
					nextTile.RemoveCurrentFromBoard();
					captureThisTurn = true;
					urGC.ShowAlertText("Captured by the enemy!");
                    if (!showingBark) {
                        urGC.TriggerBark(false, GameManager.UrCaptureText);
                        showingBark = true;
                    }

                    urGC.PlaySoundFX(UrGameController.SoundTrigger.Capture, false);
				}

				pieceToMove.ClearPossiblePath();
				urGC.UnhighlightBoard();
				pieceToMove.DestroyGhost();

				//Flip
				if (pieceToMove.BoardIndex < 16 && pieceToMove.BoardIndex + currentRoll >= 16) {
					pieceToMove.FlipPiece();
                    if (!showingBark) {
                        urGC.TriggerBark(false, GameManager.UrFlipText);
                        showingBark = true;
                    }
                }

				//Not moving onto the board for the first time
				if (pieceToMove.BoardIndex != -1) {
					urGC.enemyBoardPositions[pieceToMove.BoardIndex].ClearOccupied();
					pieceToMove.BoardIndex += currentRoll;
				} else {
					//If they're at at the beginning, we don't want to set their position to 4 if they rolled a 5
					pieceToMove.BoardIndex = 0;
                    if (!showingBark) {
                        urGC.TriggerBark(false, GameManager.UrMoveOnText);
                        showingBark = true;
                    }
                }

				//If you're moving off the board
				if (pieceToMove.BoardIndex == urGC.playerBoardPositions.Count - 1) 
				{
					//We don't have to check for other barks here because if you're moving off the board, you're not moving on, or hitting a rosette, or capturing
					urGC.TriggerBark(false, GameManager.UrMoveOffText, true);
					urGC.PointScored(false, pieceToMove);
					urGC.PlaySoundFX(UrGameController.SoundTrigger.OffBoard, false);
				}
				else {
					nextTile.SetOccupied(pieceToMove);
				}
				
				if (nextTile.isRosette) 
				{
					if (captureThisTurn) {
						urGC.ShowAlertText("Captured! And Opponent Rolls Again!");
					} else {
						urGC.ShowAlertText("Opponent Rolls Again");
					}

                    if (!showingBark) {
                        urGC.TriggerBark(false, GameManager.UrRosetteText);
                        showingBark = true;
                    }

                    urGC.PlaySoundFX(UrGameController.SoundTrigger.Rosette, false);
					redoTurn = true;
				}
			}

            StartCoroutine(urGC.WaitToSwitchTurn(!redoTurn, !redoTurn, SettingsManager.AnimationsEnabled ? endTurnPause : noAnimEndPause));

		}
	}

	private UrPiece ChoosePieceToMove(List<UrPiece> movablePieceList) 
	{
		//If there's only one piece, just return that without wasting time doing the other processing
		if (movablePieceList.Count == 1) 
		{
			//Debug.Log("Only one piece can move, taking that move");
			return movablePieceList[0];
		}

		//I've chosen to do this with one list and multiple loops for a few reasons
		//The method stops as soon as it finds a piece to move
		//Four lists would be a lot of memory when one works just fine
		//Singular for-loops are pretty efficient, especially given none of these will ever run more than 3 times
		//I find this to be nicely readable and debug-able
		List<UrPiece> potentialPieces = new List<UrPiece>();
		
		//1. Move piece off the end of the board
		for (int i = 0; i < movablePieceList.Count; i++) 
		{
			if (movablePieceList[i].BoardIndex + currentRoll == urGC.enemyBoardPositions.Count - 1) 
			{
				potentialPieces.Add(movablePieceList[i]);
			}
		}
		if (potentialPieces.Count != 0) 
		{
			Debug.Log("At least one piece can move off the board, taking that move");
			return potentialPieces.RandomElement();
		}

		//2. Capture one of the player's pieces
		for (int i = 0; i < movablePieceList.Count; i++) 
		{
			if (urGC.enemyBoardPositions[movablePieceList[i].BoardIndex + currentRoll].OppositeOccupyingPiece(false)) {
				potentialPieces.Add(movablePieceList[i]);
			}
		}
		if (potentialPieces.Count != 0) 
		{
			Debug.Log("At least one piece can capture a player piece, taking that move");
			return potentialPieces.RandomElement();
		}

		//3. Land on a rosette and roll again
		for (int i = 0; i < movablePieceList.Count; i++) 
		{
			if (urGC.enemyBoardPositions[movablePieceList[i].BoardIndex + currentRoll].isRosette) {
				potentialPieces.Add(movablePieceList[i]);
			}
		}
		if (potentialPieces.Count != 0) 
		{
			Debug.Log("At least one piece can land on a rosette, taking that move");
			return potentialPieces.RandomElement();
		}

		//4. Move a piece off of the board onto it
		for (int i = 0; i < movablePieceList.Count; i++) 
		{
			//If it's in this list, it can already be moved, so you just need to check if it's off the board
			if (movablePieceList[i].BoardIndex == -1) {
				potentialPieces.Add(movablePieceList[i]);
			}
		}
		if (potentialPieces.Count != 0) 
		{
			Debug.Log("At least one piece can move onto the board, taking that move");
			return potentialPieces.RandomElement();
		}

		//If no piece can do any of those things, choose at random

		Debug.Log("No priority moves available, moving a piece at random");
		return movablePieceList[0];
	}

}
