using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Movement & Look")]
    [SerializeField] private Movement movement;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private string slipperyTag;
    [SerializeField] private float slipDuartion;
    private float slipTimer;

    [Header("Pickup & Throw")]
    [SerializeField] private PickupItem pickupItem;

    [Header("Health")]
    [SerializeField] private Health health;

    [Header("Player Sprite Animation")]
    public Animator spriteAnimator;

    [Header("Diaper")]
    [SerializeField] private float diaperDuration;
    private float diaperTimer;

    void Start()
    {
        slipTimer = 0;
        diaperTimer = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        health.onDeath += OnDeath;
    }

    void OnDeath()
    {
        Debug.Log("Player " + playerInput.playerIndex + " has died");
        Destroy(gameObject);
    }

    void Update()
    {
        // slip timer
        if (slipTimer > 0)
        {
            slipTimer -= Time.deltaTime;
            if (slipTimer <= 0) slipTimer = 0;
        }

        // diaper timer
        if (diaperTimer > 0)
        {
            diaperTimer -= Time.deltaTime;
            if (diaperTimer <= 0)
            {
                diaperTimer = 0;
                // reset the player sprite color
                spriteAnimator.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        // walk animation
        if (rb.velocity.magnitude > 0.1f)
        {
            spriteAnimator.SetFloat("isWalking", 1);
        }
        else
        {
            spriteAnimator.SetFloat("isWalking", 0);
        }
    }

    public void GetMoveInput(CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        // flip the direction of the player if the player is diapered
        if (diaperTimer > 0) input *= -1;
        float inputVertical = input.y;
        float inputHorizontal = input.x;

        if (slipTimer > 0) movement.slide(inputHorizontal, inputVertical);
        else movement.Move(inputHorizontal, inputVertical);
    }

    public void GetLookInput(CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        // flip the direction of the player if the player is diapered
        if (diaperTimer > 0) input *= -1;
        if (input.magnitude < 0.1f) return;
        movement.LookDirection(input);

        // change the player sprite based on the look direction
        // find the closest direction to the input
        float angleY = Vector2.Angle(Vector2.up, input);
        float angleX = Vector2.Angle(Vector2.right, input);
        if (angleY < 45)
        {
            spriteAnimator.SetFloat("dirY", 1);
            spriteAnimator.SetFloat("dirX", 0);
        }
        else if (angleY > 135)
        {
            spriteAnimator.SetFloat("dirY", -1);
            spriteAnimator.SetFloat("dirX", 0);
        }
        else if (angleX < 45)
        {
            spriteAnimator.SetFloat("dirX", 1);
            spriteAnimator.SetFloat("dirY", 0);
        }
        else if (angleX > 135)
        {
            spriteAnimator.SetFloat("dirX", -1);
            spriteAnimator.SetFloat("dirY", 0);
        }
    }

    public void GetPickupInput(CallbackContext context)
    {
        if (context.started) pickupItem.Pickup();
    }

    public void GetThrowInput(CallbackContext context)
    {
        if (context.started)
        {
            spriteAnimator.SetTrigger("throw");
            pickupItem.ThrowItem();
        }
    }

    public void GetThrowDiaperInput(CallbackContext context)
    {
        if (context.started)
        {
            spriteAnimator.SetTrigger("throw");
            pickupItem.ThrowDiaper();
        }
    }

    // check if the player is on a slippery surface
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(slipperyTag))
        {
            // get the velocity direction and slide in that direction
            Vector2 direction = rb.velocity.normalized;
            movement.InitSlide(direction.x, direction.y);
            slipTimer = slipDuartion;
            Destroy(other.gameObject);
        }
    }

    public void DiaperHit()
    {
        diaperTimer = diaperDuration;

        // tint the player sprite to brown
        spriteAnimator.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.25f, 0);
    }
}
