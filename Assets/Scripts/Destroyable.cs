using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private int lootChance=5;

    void Start()
    {
        health.onDeath += OnDeath;
    }

    void OnDeath()
    {
        if (lootPrefab != null)
        {
            int random = Random.Range(0, 10);
            if (random <= lootChance) Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
