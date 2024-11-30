using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] private Health health;

    void Start()
    {
        health.onDeath += OnDeath;
    }

    void OnDeath()
    {
        Debug.Log("Object died");
        Destroy(gameObject);
    }
}
