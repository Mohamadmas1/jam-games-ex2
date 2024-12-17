using UnityEngine;

public enum PerformedAction
{
    Pickup,
    Throw,
    Diaper
}

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
    [SerializeField] private GameObject diaperPrefab;
    [SerializeField] private float diaperDuration;
    private float diaperTimer = 0;
    public Transform diaperUIMask;
    [SerializeField] private float maxMaskScale = 2.7f;

    void Update()
    {
        if (diaperTimer > 0) diaperTimer -= Time.deltaTime;
        float maskScale = Mathf.Lerp(0, maxMaskScale, 1 - diaperTimer / diaperDuration);
        diaperUIMask.localScale = new Vector3(maskScale, maskScale, 1);

        // circle cast a bigger radius to turn off the light on the items that are out of range
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius * 2);
        foreach (var hitCollider in hit)
        {
            if (hitCollider.CompareTag(throwableTag) || hitCollider.CompareTag(healthTag))
            {
                hitCollider.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        // circle cast to light up the items that can be picked up
        hit = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hit)
        {
            if (hitCollider.CompareTag(throwableTag) || hitCollider.CompareTag(healthTag))
            {
                hitCollider.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }

    public PerformedAction Action()
    {
        if (IsHoldingItem())
        {
            ThrowItem();
            return PerformedAction.Throw;
        }
        else if (diaperTimer <= 0)
        {
            ThrowDiaper();
            return PerformedAction.Diaper;
        }
        else
        {
            Pickup();
            return PerformedAction.Pickup;
        }
    }

    private void Pickup()
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
                health.IncreaseHealth(1);
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

    private void ThrowDiaper()
    {
        if (diaperTimer > 0) return;
        diaperTimer = diaperDuration;

        // instantiate the Diaper prefab
        GameObject Diaper = Instantiate(diaperPrefab, throwTransform.position, throwTransform.rotation);
        // enable the ThrownProjectile script on the item so it can deal damage
        Diaper.GetComponent<ThrownProjectile>().isEnabled = true;
        // give the Diaper a force up
        Diaper.GetComponent<Rigidbody2D>().AddForce(Diaper.transform.up * throwForce, ForceMode2D.Impulse);
    }

    private void ThrowItem()
    {
        if (!IsHoldingItem()) return;

        // set the position and rotation of the item to the throwTransform
        heldItem.position = throwTransform.position;
        heldItem.rotation = throwTransform.rotation;
        // enable the rigidbody and give it a force up
        heldItem.GetComponent<Rigidbody2D>().isKinematic = false;
        heldItem.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        heldItem.GetComponent<Collider2D>().isTrigger = false;
        heldItem.GetComponent<Rigidbody2D>().AddForce(heldItem.transform.up * throwForce, ForceMode2D.Impulse);
        // enable the ThrownProjectile script on the item so it can deal damage
        heldItem.gameObject.GetComponent<ThrownProjectile>().isEnabled = true;
        // remove the parent
        heldItem.SetParent(null);
        heldItem = null;
    }
}
