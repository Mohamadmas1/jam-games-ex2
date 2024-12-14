using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeUI : MonoBehaviour
{
    public Health health;
    public List<GameObject> hearts;

    private int currentHealth;


    private void Start()
    {
        currentHealth = hearts.Count;
    }

    void Update()
    {
        if (health == null) return;
        
        if (health.GetHealth() != currentHealth) UpdateHearts();
    }

    private void UpdateHearts()
    {
        var newHealth = health.GetHealth();
        currentHealth = health.GetHealth();
        if (newHealth > currentHealth)
        {
            hearts[currentHealth].SetActive(true);
        }
        else
        {
            hearts[currentHealth].SetActive(false);
        }
        currentHealth = newHealth;
    }
}
