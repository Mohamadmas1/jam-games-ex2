using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ant : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float minDistanceToDestination = 0.1f;
    public Vector3 destination;

    [Header("Randomness")]
    [SerializeField] private float directionRandomness = 0.1f;
    [SerializeField] private float changeDirectionChance = 0.1f;
    private Vector3 currentDirection;

    [Header("Targets")]
    [SerializeField] private string tetrisPieceTag = "Tetris Piece";
    [SerializeField] private string slidePieceTag = "Slide Piece";
    [SerializeField] private string pisaTag = "Pisa";

    private void Start()
    {
        currentDirection = (destination - transform.position);
        currentDirection.z = 0.0f;
        currentDirection.Normalize();
    }

    private void Update()
    {
        // Move the ant towards the destination
        Vector3 dir = currentDirection;
        transform.position += speed * Time.deltaTime * dir;

        // orient the ant towards the direction it is moving
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle - 90.0f);
    }

    private void FixedUpdate()
    {
        // Randomly change the direction of the ant
        float roll = Random.Range(0.0f, 1.0f);
        if (roll <= changeDirectionChance)
        {
            // Randomize the direction, the farther from the destination the more randomness
            Vector3 dir = destination - transform.position;
            dir.z = 0.0f;
            float distance = dir.magnitude;
            // the angle between the random direction and the destination direction is <= distance * directionRandomness
            float angle = Random.Range(-distance * directionRandomness, distance * directionRandomness);
            Vector3 randomDir = Quaternion.Euler(0.0f, 0.0f, angle) * dir;
            currentDirection = randomDir.normalized;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tetrisPieceTag))
        {
            ColosseumTetrisPiece tetrisPiece = collision.transform.parent.GetComponent<ColosseumTetrisPiece>();
            tetrisPiece.ThrowRandomely();
        }
        else if (collision.gameObject.CompareTag(slidePieceTag))
        {
            VaticanSlidePiece slidePiece = collision.gameObject.GetComponent<VaticanSlidePiece>();
            slidePiece.puzzle.DoRandomMove();
        }
        else if (collision.gameObject.CompareTag(pisaTag))
        {
            // TODO: Implement the Pisa piece collision
        }

        Die();
    }


    public void EventOnPointerDown(BaseEventData data)
    {
        health--;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
