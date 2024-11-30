using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    [Header("Mouse Look")]
    [SerializeField] private Transform rotationTransform;

    public void Move(float x, float y)
    {
        Vector3 up = rb.transform.up * y * speed;
        Vector3 right = rb.transform.right * x * speed;
        rb.velocity = new Vector3(up.x + right.x, up.y + right.y);
    }

    public void LookDirection(Vector2 direction)
    {
        Vector3 lookDirection = new Vector3(direction.x, direction.y, 0);
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rotationTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void LookAt(Vector2 target)
    {
        Vector2 direction = target - new Vector2(rotationTransform.position.x, rotationTransform.position.y);
        LookDirection(direction);
    }
}
