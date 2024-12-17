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
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(true);
        }
    }

    void Update()
    {
        if (health == null) return;

        if (health.health != currentHealth)
        {
            if (health.health < currentHealth)
            {
                for (int i = currentHealth - 1; i >= health.health; i--)
                {
                    hearts[i].SetActive(false);
                }
            }
            else
            {
                for (int i = currentHealth; i < health.health; i++)
                {
                    hearts[i].SetActive(true);
                }
            }

            currentHealth = health.health;
        }
    }
}
