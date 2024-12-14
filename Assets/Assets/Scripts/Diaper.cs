using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diaper : MonoBehaviour
{
    public void OnHit(Collision2D collision)
    {
        // check if the object has a health component
        //check if the object has a player controller component
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.DiaperHit();
        }
    }
}
