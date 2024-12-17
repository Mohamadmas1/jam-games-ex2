using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private int lootChance = 50;
    public bool shouldDestroy = false;

    void Start()
    {
        health.onDeath += OnDeath;
    }

    void OnDeath()
    {
        if (lootPrefab != null)
        {
            int random = Random.Range(0, 100);
            if (random <= lootChance) Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }
        var animator = GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Destroy");
        else Destroy(gameObject);
    }

    public void Update()
    {
        if (shouldDestroy) Destroy(gameObject);
    }
}
