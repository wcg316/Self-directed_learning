using UnityEngine;
using System.Collections;
using static UnityEngine.Random;
using Unity.Collections;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum ColliderShape
{
    Stand,
    RunRight,
    RunLeft
}

[System.Serializable]
public class EffectProperties
{
    [SerializeField] GameObject effect;
    public GameObject Effect
    {
        get => effect;
    }
    AudioSource audioSource;
    [SerializeField] AudioClip sound;
    int horizonalDirectionMultiplier;
    [SerializeField] int baseAngle;
    [SerializeField] int offsetAngle;
    [SerializeField] float distanceX;
    [SerializeField] float distanceY;
    [SerializeField] float duration;
    public float Duration
    {
        get => duration;
    }
    [SerializeField] float cooldownDuration;
    public float CooldownDuration
    {
        get => cooldownDuration;
    }
    bool onCooldown;
    public bool OnCooldown
    {
        get => onCooldown;
    }

    public void Initialize()
    {
        if (effect != null)
        {
            audioSource = effect.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = effect.AddComponent<AudioSource>();

            effect.SetActive(false);
        }
    }

    public void Activate()
    {
        effect.SetActive(true);
    }

    public void Hide()
    {
        effect.SetActive(false);
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(sound);
    }

    public void StartCooldown()
    {
        onCooldown = true;
    }

    public void EndCooldown()
    {
        onCooldown = false;
    }

    public void SetHorizontalDirectionMultiplier(int multiplier)
    {
        horizonalDirectionMultiplier = multiplier;
    }

    public void SetPositionFrom(Transform transform)
    {
        Vector3 offset = new Vector3(distanceX * horizonalDirectionMultiplier, distanceY, 0f);
        effect.transform.localPosition = transform.position + offset;
    }

    public void SetAngle()
    {
        int randomAngle = Range(-offsetAngle, offsetAngle + 1);

        effect.transform.eulerAngles =
            new Vector3(0f, 0f, baseAngle * horizonalDirectionMultiplier + randomAngle);
    }

    public Vector3 GetOffset()
    {
        return new Vector3(distanceX * horizonalDirectionMultiplier, 0, 0);
    }

}

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private ColliderController colliderController;
    private PlayerAnimation playerAnimation;
    public AudioSource footstepSoundSource;
    public AudioClip[] footstepSounds;
    private float footstepSoundInterval = 0.35f;
    private bool isPlayingFootstepSound = false;
    public LayerMask groundLayer;
    private const float EXTRA_HEIGHT = 1.4f;
    private int horizontalDirectionMultiplier = 1;
    public bool isGrounded = true;
    public float moveSpeed;
    public float dashForce;
    private const float DASH_DURATION = 0.15f;
    private bool isDashing = false;
    public float dashCooldownDuration;
    public bool canDashInAir = false;
    public bool onDashCooldown = false;
    public float jumpForce;
    public bool playingFootstepSound;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool onAttackCooldown = false;
    public EffectProperties normalAttack;
    public EffectProperties dashDust;

    void Start()
    {
        colliderController = GetComponent<ColliderController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        footstepSoundSource.volume = 0.1f;
        transform.position = new Vector2(0f, -0.45f);
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        normalAttack.Initialize();
        dashDust.Initialize();
        colliderController.InitializeColliders();
    }

    void Update()
    {
        ForTesting();

        Variable();
        Move();
        Jump();
        Dash();
        Attack();
        Die();
    }

    void ForTesting()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TeleportToOrigin();
        }
        Debug.DrawRay(transform.position, Vector2.down * EXTRA_HEIGHT, Color.red);
    }

    void TeleportToOrigin()
    {
        body.linearVelocity = Vector2.zero;
        transform.position = new Vector2(0f, -0.45f);
    }

    void Variable()
    {
        isGrounded = CheckIfIsGrounded();
        moveSpeed = 10f;
        dashForce = 100f;
        jumpForce = 30f;
        dashCooldownDuration = 0.5f;
    }

    void Move()
    {
        bool canMove = !isDashing;

        if (Input.GetKey(KeyCode.D) && canMove)
        {
            MoveInDirection(Direction.Right);
        }
        else if (Input.GetKey(KeyCode.A) && canMove)
        {
            MoveInDirection(Direction.Left);
        }
        else
        {
            StopMovement();
        }
    }

    void MoveInDirection(Direction direction)
    {
        FaceDirection(direction);
        SetHorizontalDirectionMultiplier();
        AdjustColliderWhileRunning(direction);
        MoveForwardWithSpeed(
            moveSpeed * Time.deltaTime * horizontalDirectionMultiplier
        );

        PlayAnimation("run");

        if (isGrounded)
        {
            PlayFootstepSound(footstepSounds, 0);
        }
    }

    void FaceDirection(Direction direction)
    {
        // 因為true代表向左，false代表向右
        spriteRenderer.flipX = direction == Direction.Left;
    }

    void SetHorizontalDirectionMultiplier()
    {
        horizontalDirectionMultiplier =
            spriteRenderer.flipX ? -1 : 1;
    }

    void AdjustColliderWhileRunning(Direction direction)
    {
        ColliderShape colliderShape = GetTargetColliderIndexWhileMoving(direction);

        AdjustColliderTo(colliderShape);
    }

    ColliderShape GetTargetColliderIndexWhileMoving(Direction direction)
    {
        return direction switch
        {
            Direction.Right => ColliderShape.RunRight,
            Direction.Left => ColliderShape.RunLeft,
            _ => 0
        };
    }

    void AdjustColliderTo(ColliderShape colliderShape)
    {
        int idx = (int)colliderShape;

        if (colliderController.colliders[idx].Enabled == false)
        {
            colliderController.FindTheOnlyOneColliderEnabled().enabled = false;
            colliderController.EnableColliderByIndex(idx);
        }
    }

    void StopMovement()
    {
        AdjustColliderTo(ColliderShape.Stand);

        StopAnimation("run");
        footstepSoundSource.Stop();
    }

    void MoveForwardWithSpeed(float speed)
    {
        transform.Translate(speed, 0, 0);
    }

    void PlayAnimation(string name)
    {
        animator.SetBool(name, true);
    }

    void StopAnimation(string name)
    {
        animator.SetBool(name, false);
    }

    void PlayFootstepSound(AudioClip[] sounds, int idx)
    {
        if (sounds.Length > idx && !isPlayingFootstepSound)
        {
            StartCoroutine(FootstepSoundCoroutie(sounds[idx]));
        }
    }

    IEnumerator FootstepSoundCoroutie(AudioClip sound)
    {
        isPlayingFootstepSound = true;
        footstepSoundSource.PlayOneShot(sound);

        yield return new WaitForSeconds(footstepSoundInterval);

        isPlayingFootstepSound = false;

    }

    void Jump()
    {
        bool playerCanJump = CheckIfPlayerCanJump();
        bool playerJumped = Input.GetKeyDown(KeyCode.W) && playerCanJump;

        if (playerJumped)
        {
            ClearVerticalForce();
            ApplyImpulseForceToDirection(jumpForce, Direction.Up);
        }
    }

    bool CheckIfPlayerCanJump()
    {
        return isGrounded && !isDashing;
    }

    void ClearVerticalForce()
    {
        body.linearVelocityY = 0f;
    }

    void ClearHorizontalForce()
    {
        body.linearVelocityX = 0f;
    }

    void ApplyImpulseForceToDirection(float force, Direction direction)
    {
        Vector2 vector2 = direction switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Right => Vector2.right,
            Direction.Left => Vector2.left,
            _ => Vector2.zero
        };

        body.AddForce(vector2 * force, ForceMode2D.Impulse);
    }


    void Dash()
    {
        bool playerCanDash = CheckIfPlayerCanDash();
        bool playerDashed = Input.GetKeyDown(KeyCode.K) && playerCanDash;

        if (playerDashed)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    bool CheckIfPlayerCanDash()
    {
        return !onDashCooldown && (isGrounded || canDashInAir);
    }

    IEnumerator DashCoroutine()
    {
        onDashCooldown = true;
        isDashing = true;
        Direction direction = GetHorizontalDirection();

        ApplyImpulseForceToDirection(dashForce, direction);
        PlayAnimation("dash");

        yield return new WaitForSeconds(DASH_DURATION);
        ClearHorizontalForce();
        StopAnimation("dash");
        isDashing = false;

        yield return new WaitForSeconds(dashCooldownDuration);
        onDashCooldown = false;
    }

    Direction GetHorizontalDirection()
    {
        return spriteRenderer.flipX ? Direction.Left : Direction.Right;
    }

    void Attack()
    {
        bool playerAttacked = Input.GetKeyDown(KeyCode.J) && !normalAttack.OnCooldown;

        if (playerAttacked)
        {
            StartCoroutine(EffectCoroutine(normalAttack));
        }
    }

    IEnumerator EffectCoroutine(EffectProperties effect)
    {
        effect.SetHorizontalDirectionMultiplier(horizontalDirectionMultiplier);

        effect.SetPositionFrom(transform);
        effect.SetAngle();

        effect.Activate();
        effect.PlaySound();

        effect.StartCooldown();

        yield return new WaitForSeconds(effect.Duration);
        effect.Hide();

        yield return new WaitForSeconds(effect.CooldownDuration);
        effect.EndCooldown();
    }

    bool CheckIfIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, EXTRA_HEIGHT, groundLayer);

        return hit.collider != null;
    }

    void Die()
    {
        bool playerDied = transform.position.y < -40f;

        if (playerDied)
        {
            TeleportToOrigin();
        }
    }

}