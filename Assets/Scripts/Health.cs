using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    public int health
    {
        get => _health;
        private set => _health = value;
    }
    // list of functions that will be called when the health reaches 0
    public delegate void OnDeath();
    public OnDeath onDeath;

    // onHit
    public delegate void OnHit();
    public OnHit onHit;

    private int initialHealth;

    private void Start()
    {
        initialHealth = _health;
    }

    public void IncreaseHealth(int amount)
    {
        _health = Mathf.Min(_health + amount, initialHealth);
    }

    public void DecreaseHealth(int amount)
    {
        _health -= amount;
        onHit?.Invoke();
        if (_health <= 0)
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
