using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement & Look")]
    [SerializeField] private Movement movement;

    [Header("Pickup & Throw")]
    [SerializeField] private PickupItem pickupItem;

    [Header("Health")]
    [SerializeField] private Health health;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        health.onDeath += OnDeath;
    }

    void OnDeath()
    {
        Debug.Log("Player died");
        Destroy(gameObject);
    }

    void Update()
    {
        // Toggle cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Pickup or throw item
        if (Input.GetKeyDown(KeyCode.E)) pickupItem.Pickup();
        if (Input.GetKeyDown(KeyCode.F)) pickupItem.Throw();
    }

    void FixedUpdate()
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        movement.Move(inputHorizontal, inputVertical);
    }
}
