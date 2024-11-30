using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    public bool isEnabled = false;
    private bool hasHit = false;

    // check when the projectile hits something
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit || !isEnabled) return;

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
}
