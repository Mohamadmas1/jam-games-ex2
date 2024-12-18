using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThrownProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject puddlePrefab;
    public bool isEnabled = false;
    private bool hasHit = false;
    [SerializeField] private UnityEvent<Collision2D> onHit;

    // check when the projectile hits something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit || !isEnabled) return;
        onHit.Invoke(collision);

        // check if the object has a health component
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.DecreaseHealth(damage);
        }

        // destroy the projectile
        Destroy(gameObject);
        hasHit = true;
    }

    void FixedUpdate()
    {
        if (!isEnabled) return;

        if (GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f)
        {
            if (puddlePrefab != null) Instantiate(puddlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
