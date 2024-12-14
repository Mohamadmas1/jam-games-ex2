using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float initSlideForce;
    [SerializeField] private float slideForce;
    [SerializeField] private float slideDrag;

    [Header("Mouse Look")]
    [SerializeField] private Transform rotationTransform;

    public void Move(float x, float y)
    {
        rb.velocity = new Vector3(x * speed, y * speed);
    }

    public void InitSlide(float x, float y)
    {
        Vector3 upForce = rb.transform.up * y * initSlideForce;
        Vector3 rightForce = rb.transform.right * x * initSlideForce;
        rb.AddForce(new Vector3(upForce.x + rightForce.x, upForce.y + rightForce.y), ForceMode2D.Impulse);
    }

    public void slide(float x, float y)
    {
        // slide input
        Vector3 upForce = rb.transform.up * y * slideForce;
        Vector3 rightForce = rb.transform.right * x * slideForce;
        rb.AddForce(new Vector3(upForce.x + rightForce.x, upForce.y + rightForce.y));

        // slide drag
        rb.velocity = rb.velocity * slideDrag;
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
