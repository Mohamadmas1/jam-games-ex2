using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaticanSlidePuzzle : MonoBehaviour
{
    [SerializeField] private Vector2Int puzzleSize;
    [SerializeField] private List<VaticanSlidePiece> pieces;
    private Vector2Int emptyPiece;

    private void Start()
    {
        // initialize the puzzle, make the empty piece in the last position
        emptyPiece = new Vector2Int(puzzleSize.x - 1, puzzleSize.y - 1);
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].puzzle = this;
            pieces[i].correctPosition = new Vector2Int(i % puzzleSize.x, i / puzzleSize.x);
            SetPiecePosition(pieces[i], new Vector2Int(i % puzzleSize.x, i / puzzleSize.x));
        }

        // randomize the puzzle by making a bunch of random slides
        for (int i = 0; i < 100; i++)
        {
            DoRandomMove(false);
        }
    }

    public bool SlidePiece(VaticanSlidePiece piece, bool checkSolved = true)
    {
        // check if the piece is next to the empty piece
        int distanceFromEmpty = Mathf.Abs(piece.currentPosition.x - emptyPiece.x) + Mathf.Abs(piece.currentPosition.y - emptyPiece.y);
        if (distanceFromEmpty != 1) return false;

        // swap the piece with the empty piece
        Vector2Int temp = piece.currentPosition;
        SetPiecePosition(piece, emptyPiece);
        emptyPiece = temp;

        // check if the puzzle is solved
        if (checkSolved && IsPuzzleSolved())
        {
            Debug.Log("Puzzle solved!");
        }

        return true;
    }

    public void DoRandomMove(bool checkSolved = true)
    {
        while (true)
        {
            int randomPieceIndex = Random.Range(0, pieces.Count);
            if (SlidePiece(pieces[randomPieceIndex], checkSolved)) break;
        }
    }

    private void SetPiecePosition(VaticanSlidePiece piece, Vector2Int position)
    {
        piece.currentPosition = position;
        piece.transform.localPosition = new Vector3(piece.currentPosition.x, piece.currentPosition.y, 0);
    }

    public bool IsPuzzleSolved()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].correctPosition != pieces[i].currentPosition) return false;
        }
        return true;
    }
}
