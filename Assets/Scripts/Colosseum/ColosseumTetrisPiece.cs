using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ColosseumTetrisPiece : MonoBehaviour
{
    public ColosseumTetrisPuzzle puzzle;
    public bool IsPlaced = false;
    public Vector2Int currentPlacement;
    public Vector2Int[] occupiedSlots;
    private Vector3 initialPosition;

    public void BeginDrag(BaseEventData eventData)
    {
        initialPosition = transform.position;

        // set all child colliders to trigger
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders) collider.isTrigger = true;
    }

    public void Drag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = (PointerEventData)eventData;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    public void EndDrag(BaseEventData eventData)
    {
        // set all child colliders to non-trigger
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders) collider.isTrigger = false;

        // get the Vector2Int local position of the piece
        Vector2Int position = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.y));

        // the new position is outside the puzzle
        if (position.x < 0 || position.x >= puzzle.size.x || position.y < 0 || position.y >= puzzle.size.y)
        {
            // if the piece is touching the puzzle, return it to its initial position
            if (!IsCompletelyOutside(transform.position)) transform.position = initialPosition;
            // if the piece is completely outside the puzzle and it was placed, remove it
            else if (IsPlaced) puzzle.RemovePiece(this, currentPlacement);
        }
        // the new position is inside the puzzle
        else
        {
            // the piece is already placed
            if (IsPlaced)
            {
                puzzle.RemovePiece(this, currentPlacement);

                // if the placement failed, return the piece to its initial placement
                if (!puzzle.PlacePiece(this, position))
                {
                    puzzle.PlacePiece(this, currentPlacement);
                }
            }
            // the piece is not already placed
            else
            {
                // if the placement failed, return the piece to its initial position
                if (!puzzle.PlacePiece(this, position))
                {
                    transform.position = initialPosition;
                }
            }
        }

        // snap the piece to the grid if it's placed
        if (IsPlaced) transform.localPosition = new Vector3(currentPlacement.x, currentPlacement.y, 0);
    }

    public bool IsCompletelyOutside(Vector3 position)
    {
        bool isOutside = true;

        // convert the position to local position
        Vector3 localPosition = transform.parent.InverseTransformPoint(position);

        foreach (Vector2Int slot in occupiedSlots)
        {
            // get the 4 corners of the slot
            for (float i = -1; i <= 1; i += 2)
            {
                for (float j = -1; j <= 1; j += 2)
                {
                    // get the corner local position
                    Vector3 corner = localPosition + new Vector3(slot.x + i * 0.5f, slot.y + j * 0.5f, 0);
                    // get the corner grid position
                    Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(corner.x), Mathf.RoundToInt(corner.y));
                    // check if the corner is inside the puzzle
                    if (gridPosition.x >= 0 && gridPosition.x < puzzle.size.x && gridPosition.y >= 0 && gridPosition.y < puzzle.size.y)
                    {
                        isOutside = false;
                        break;
                    }
                }
                if (!isOutside) break;
            }
            if (!isOutside) break;
        }

        return isOutside;
    }

    public void ThrowRandomely()
    {
        // generate a random position inside the camera view but outside the puzzle
        Vector3 randomPosition;
        while (true)
        {
            // generate a random position inside the camera view
            float x = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x, Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
            float y = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
            randomPosition = new Vector3(x, y, transform.position.z);

            // the new position is outside the puzzle
            if (IsCompletelyOutside(randomPosition)) break;
        }

        if (IsPlaced) puzzle.RemovePiece(this, currentPlacement);
        transform.position = randomPosition;
    }
}
