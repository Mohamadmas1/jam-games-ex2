using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControllerUpdated : MonoBehaviour
{
    
    [Header("Movement & Look")]
    [SerializeField] private Movement movement;
    
    [Header("Pickup & Throw")]
    [SerializeField] private PickupItem pickupItem;
    
    [Header("Health")]
    [SerializeField] private Health health;
    
    public float moveSpeed = 5f;
    // public string horizontalAxis = "Horizontal";
    // public string verticalAxis = "Vertical";
    private Vector2 moveDirection;

    private Rigidbody2D rb; 
    public InputAction PlayerMovement;
  
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        health.onDeath += OnDeath;
        
        rb = GetComponent<Rigidbody2D>();
    }
    
    void OnDeath()
    {
        Debug.Log("Player died");
        Destroy(gameObject);
    }
    
    
    private void OnEnable()
    {
        PlayerMovement.Enable();
    }
    private void OnDisable()
    {
        PlayerMovement.Disable();
    }
    void Update()
    {
        moveDirection = PlayerMovement.ReadValue<Vector2>();
    
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
        if (CompareTag("Player1"))
        {
            if (Input.GetKeyDown(KeyCode.E)) pickupItem.Pickup();
            if (Input.GetKeyDown(KeyCode.F)) pickupItem.ThrowItem();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P)) pickupItem.Pickup();
            if (Input.GetKeyDown(KeyCode.O)) pickupItem.ThrowItem();
        }
        
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
 
}