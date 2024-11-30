using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform throwTransform;
    [SerializeField] private float radius = 5f;
    [SerializeField] private string throwableTag = "Throwable";
    [SerializeField] private string healthTag = "Health";
    private Transform heldItem = null;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private Health health;
    [SerializeField] private GameObject jellyPrefab;

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }

    public void Pickup()
    {
        if (IsHoldingItem()) return;

        // get all the colliders in the radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            // check if the collider has the tag "Health" and pick the first one
            if (hitCollider.CompareTag(healthTag))
            {
                // increase the health of the player
                Destroy(hitCollider.gameObject);
                health.IncreaseHealth(10);
            }

            // check if the collider has the tag "Throwable" and only pick up one item
            if (hitCollider.CompareTag(throwableTag))
            {
                if (IsHoldingItem()) return;
                heldItem = hitCollider.transform;

                // set the position and rotation of the item to the hand
                hitCollider.transform.position = handTransform.position;
                hitCollider.transform.rotation = handTransform.rotation;
                hitCollider.transform.SetParent(handTransform);
                // disable the rigidbody
                hitCollider.GetComponent<Rigidbody2D>().isKinematic = true;
                hitCollider.isTrigger = true;
            }
        }
    }

    public void Throw()
    {
        if (!IsHoldingItem()) ThrowJelly();
        else ThrowItem();
    }

    private void ThrowJelly()
    {
        // instantiate the jelly prefab
        GameObject jelly = Instantiate(jellyPrefab, throwTransform.position, throwTransform.rotation);
        // enable the ThrownProjectile script on the item so it can deal damage
        jelly.gameObject.GetComponent<ThrownProjectile>().isEnabled = true;
        // give the jelly a force up
        jelly.GetComponent<Rigidbody2D>().AddForce(transform.up * throwForce, ForceMode2D.Impulse);
    }

    private void ThrowItem()
    {
        // set the position and rotation of the item to the throwTransform
        heldItem.position = throwTransform.position;
        heldItem.rotation = throwTransform.rotation;
        // enable the rigidbody and give it a force up
        heldItem.GetComponent<Rigidbody2D>().isKinematic = false;
        heldItem.GetComponent<Collider2D>().isTrigger = false;
        heldItem.GetComponent<Rigidbody2D>().AddForce(transform.up * throwForce, ForceMode2D.Impulse);
        // enable the ThrownProjectile script on the item so it can deal damage
        heldItem.gameObject.GetComponent<ThrownProjectile>().isEnabled = true;
        // remove the parent
        heldItem.SetParent(null);
        heldItem = null;
    }
}