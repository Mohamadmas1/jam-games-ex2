using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    // list of functions that will be called when the health reaches 0
    public delegate void OnDeath();
    public OnDeath onDeath;

    private int startLife;

    private void Start()
    {
        startLife = health;
    }

    public void IncreaseHealth(int amount)
    {
        if (health + amount > startLife)
            health = startLife;
        else 
            health += amount;
    }

    public void DecreaseHealth(int amount)
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
    
    public int GetHealth()
    {
        return health;
    }
}
