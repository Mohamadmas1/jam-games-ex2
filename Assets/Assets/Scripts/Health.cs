using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    // list of functions that will be called when the health reaches 0
    public delegate void OnDeath();
    public OnDeath onDeath;


    public void IncreaseHealth(float amount)
    {
        health += amount;
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // call all the functions in the list
        onDeath?.Invoke();
    }
}
