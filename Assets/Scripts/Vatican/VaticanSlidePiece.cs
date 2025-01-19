using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaticanSlidePiece : MonoBehaviour
{
    public VaticanSlidePuzzle puzzle;
    public Vector2Int correctPosition;
    public Vector2Int currentPosition;

    public void OnClick()
    {
        puzzle.SlidePiece(this);
    }
}
