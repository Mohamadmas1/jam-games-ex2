using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2test : MonoBehaviour
{
    [SerializeField] private Health health;

    void Start()
    {
        health.onDeath += OnDeath;
    }

    void OnDeath()
    {
        Debug.Log("Player 2 died");
        Destroy(gameObject);
    }
}
