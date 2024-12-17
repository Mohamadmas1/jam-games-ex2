using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] private float cookingTime = 10f;
    private float cookingTimer = 0f;
    [SerializeField] private Vector2 xRange = new Vector2(-5f, 5f);
    [SerializeField] private Vector2 yRange = new Vector2(-5f, 5f);
    [SerializeField] private GameObject daysaPrefab;
    [SerializeField] private List<String> avoidedTags = new List<String>();
    private GameObject thrownDaysa = null;
    private Vector2 throwTarget;
    [SerializeField] private float animationDuration = 2.6f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip throwSound;


    private void Update()
    {
        cookingTimer += Time.deltaTime;
        if (cookingTimer >= cookingTime)
        {
            ThrowDaysa();
            cookingTimer = 0f;
        }

        // lerp the daysa to the target position
        if (thrownDaysa != null)
        {
            float step = Vector2.Distance(transform.position, throwTarget) / animationDuration * Time.deltaTime;
            Vector2 currentPos = new Vector2(thrownDaysa.transform.position.x, thrownDaysa.transform.position.y);
            Vector2 newPos = Vector2.MoveTowards(currentPos, throwTarget, step);
            thrownDaysa.transform.position = new Vector3(newPos.x, newPos.y, thrownDaysa.transform.position.z);

            if (newPos == throwTarget)
            {
                thrownDaysa = null;
            }
        }
    }

    private void ThrowDaysa()
    {
        // Physics2D.OverlapPoint to check if the daysa will land on a table (avoidedTags) reroll if it does
        while (true)
        {
            throwTarget = new Vector2(UnityEngine.Random.Range(xRange.x, xRange.y), UnityEngine.Random.Range(yRange.x, yRange.y));
            Collider2D hit = Physics2D.OverlapPoint(throwTarget);
            if (hit == null || !avoidedTags.Contains(hit.tag))
            {
                break;
            }
        }

        thrownDaysa = Instantiate(daysaPrefab, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetTrigger("Throw");
        audioSource.PlayOneShot(throwSound);
    }
}
