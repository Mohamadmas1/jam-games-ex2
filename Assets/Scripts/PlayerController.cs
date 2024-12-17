using System.Collections.Generic;
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

    [HideInInspector] public Animator spriteAnimator;

    [Header("Sounds")]
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip diaperSound;
    [HideInInspector] public List<AudioClip> throwSounds;
    [HideInInspector] public List<AudioClip> hitSounds;

    [Header("Diaper")]
    [SerializeField] private float diaperDuration;
    private float diaperTimer;
    public bool inputEnabled = true;

    [Header("Daysa")]
    [SerializeField] private string daysaTag;
    [SerializeField] private int daysaDamage;

    void Start()
    {
        slipTimer = 0;
        diaperTimer = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        health.onDeath += OnDeath;
        health.onHit += OnHit;
    }

    public void OnDeath()
    {
        Debug.Log("Player " + playerInput.playerIndex + " has died");
        GameManager.instance.EndGame(playerInput.playerIndex);
        // Destroy(gameObject);
    }

    public void OnHit()
    {
        audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count)]);
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
            if (audioSource.clip == null)
            {
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        else
        {
            spriteAnimator.SetFloat("isWalking", 0);
            audioSource.clip = null;
        }
    }

    public void GetMoveInput(CallbackContext context)
    {
        if (!inputEnabled) return;

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
        if (!inputEnabled) return;

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

    public void GetInteractInput(CallbackContext context)
    {
        if (!context.started || !inputEnabled) return;

        PerformedAction action = pickupItem.Action();
        if (action == PerformedAction.Diaper || action == PerformedAction.Throw)
        {
            spriteAnimator.SetTrigger("throw");
            audioSource.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Count)]);
        }
    }

    // check if the player is on a slippery surface/Daysa
    void OnTriggerEnter2D(Collider2D other)
    {
        // take damage
        if (other.CompareTag(daysaTag))
        {
            health.DecreaseHealth(daysaDamage);
        }
        // slip
        if (other.CompareTag(slipperyTag) || other.CompareTag(daysaTag))
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
        audioSource.PlayOneShot(diaperSound);

        // tint the player sprite to brown
        spriteAnimator.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.25f, 0);
    }
}
