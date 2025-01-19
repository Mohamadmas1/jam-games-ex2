using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColosseumTetrisPuzzle : MonoBehaviour
{
    public Vector2Int size;
    private ColosseumTetrisPiece[,] grid;
    [SerializeField] private ColosseumTetrisPiece[] pieces;

    private void Awake()
    {
        grid = new ColosseumTetrisPiece[size.x, size.y];
        foreach (ColosseumTetrisPiece piece in pieces) piece.puzzle = this;
    }

    public bool PlacePiece(ColosseumTetrisPiece piece, Vector2Int position)
    {
        if (piece.IsPlaced) return false;

        // check if the piece fits
        foreach (Vector2Int slot in piece.occupiedSlots)
        {
            Vector2Int gridPosition = position + slot;
            if (gridPosition.x < 0 || gridPosition.x >= size.x || gridPosition.y < 0 || gridPosition.y >= size.y) return false;
            if (grid[gridPosition.x, gridPosition.y] != null) return false;
        }

        // fill the grid with the piece
        foreach (Vector2Int slot in piece.occupiedSlots)
        {
            Vector2Int gridPosition = position + slot;
            grid[gridPosition.x, gridPosition.y] = piece;
        }
        piece.IsPlaced = true;
        piece.currentPlacement = position;

        // check if the puzzle is solved
        if (IsSolved())
        {
            Debug.Log("Puzzle solved!");
        }

        return true;
    }

    public void RemovePiece(ColosseumTetrisPiece piece, Vector2Int position)
    {
        if (!piece.IsPlaced) return;

        // remove the piece from the grid
        foreach (Vector2Int slot in piece.occupiedSlots)
        {
            Vector2Int gridPosition = position + slot;
            grid[gridPosition.x, gridPosition.y] = null;
        }
        piece.IsPlaced = false;
    }

    public bool IsSolved()
    {
        foreach (ColosseumTetrisPiece piece in pieces)
        {
            if (!piece.IsPlaced)
            {
                return false;
            }
        }

        return true;
    }
}
