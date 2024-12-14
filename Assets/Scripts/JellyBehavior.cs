using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBehavior : MonoBehaviour
{
    [SerializeField] private GameObject outerJelly;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            outerJelly.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            outerJelly.SetActive(false);
        }
    }
}
