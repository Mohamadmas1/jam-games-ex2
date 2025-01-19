using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntsManager : MonoBehaviour
{
    public void WaterBucketClicked()
    {
        // physics overlap circle to find ants in range with tag "Ant"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Ant")
            {
                collider.GetComponent<Ant>().Die();
            }
        }
    }
}
