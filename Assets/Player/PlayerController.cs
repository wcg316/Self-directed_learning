using UnityEngine;
using System.Collections;
using static UnityEngine.Random;
using Unity.Collections;
using Unity.Burst.CompilerServices;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private PlayerStatus status;
    private ColliderController colliderController;
    private PlayerAnimation playerAnimation;
    private InputManager inputManager;
    public AudioSource footstepSoundSource;
    public AudioClip[] footstepSounds;
    private float footstepSoundInterval = 0.35f;
    private bool isPlayingFootstepSound = false;
    public LayerMask groundLayer;
    private const float EXTRA_HEIGHT = 1.4f;
    private int horizontalDirectionMultiplier = 1;
    private const float DASH_DURATION = 0.15f;
    public bool playingFootstepSound;
    private Rigidbody2D body;
    private Animator animator;
    public bool onAttackCooldown = false;
    public EffectPlayer normalAttack;
    public EffectPlayer dashDust;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        colliderController = GetComponent<ColliderController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        status = PlayerStatus.Instance;
        inputManager = InputManager.Instance;

        footstepSoundSource.volume = 0.1f;
        transform.position = new Vector2(0f, -0.45f);

        groundLayer = LayerMask.GetMask("Ground");

        normalAttack.Initialize();
        dashDust.Initialize();

        colliderController.InitializeColliders();

        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        SubscribeToInputEvents();
    }

    void SubscribeToInputEvents()
    {
        inputManager.OnMovePressed += Move;
        inputManager.OnMoveReleased += StopMoving;
        inputManager.OnJumpPressed += Jump;
        inputManager.OnDashPressed += Dash;
        inputManager.OnAttackPressed += Attack;
        inputManager.OnClimbPressed += Climb;
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    void UnsubscribeFromEvents()
    {
        UnsubscribeFromInputEvents();
    }

    void UnsubscribeFromInputEvents()
    {
        inputManager.OnMovePressed -= Move;
        inputManager.OnMoveReleased -= StopMoving;
        inputManager.OnJumpPressed -= Jump;
        inputManager.OnDashPressed -= Dash;
        inputManager.OnAttackPressed -= Attack;
    }

    void Update()
    {
        ForTesting();

        Variable();
        Die();
    }

    void ForTesting()
    {
        Debug.DrawRay(transform.position, Vector2.down * EXTRA_HEIGHT, Color.red);
    }

    void TeleportToOrigin()
    {
        body.linearVelocity = Vector2.zero;
        transform.position = new Vector2(0f, -0.45f);
    }

    void Variable()
    {
        if (status != null)
        {
            status.IsGrounded = CheckIfIsGrounded();
            status.MoveSpeed = 10f;
        }
        else
        {
            Debug.LogError("PlayerStatus is not initialized.");
        }
        status.DashForce = 60f;
        status.JumpForce = 30f;
        status.DashCooldownDuration = 0.5f;
    }

    void Move(Direction direction)
    {
        bool canMove = CheckIfCanMove();

        if (canMove)
        {
            FaceDirection(direction);
            SetHorizontalDirectionMultiplier();
            AdjustColliderWhileRunning(direction);
            MoveForwardWithSpeed(
                status.MoveSpeed * Time.deltaTime * horizontalDirectionMultiplier
            );

            PlayAnimation("run");

            if (status.IsGrounded)
            {
                PlayFootstepSound(footstepSounds, 0);
            }
        }
        else
        {
            StopMoving();
        }
    }

    bool CheckIfCanMove()
    {
        return !status.IsDashing && status.isClimbing;
    }

    void FaceDirection(Direction direction)
    {
        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * (direction == Direction.Left ? -1 : 1);
        transform.localScale = scale;
    }

    void SetHorizontalDirectionMultiplier()
    {
        horizontalDirectionMultiplier =
            transform.localScale.x < 0 ? -1 : 1;
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

    void StopMoving()
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
            StartCoroutine(FootstepSoundCoroutine(sounds[idx]));
        }
    }

    IEnumerator FootstepSoundCoroutine(AudioClip sound)
    {
        isPlayingFootstepSound = true;
        footstepSoundSource.PlayOneShot(sound);

        yield return new WaitForSeconds(footstepSoundInterval);

        isPlayingFootstepSound = false;

    }

    void Jump()
    {
        bool canJump = CheckIfCanJump();

        if (canJump)
        {
            ClearVerticalForce();
            ApplyImpulseForceToDirection(status.JumpForce, Direction.Up);
        }
    }

    bool CheckIfCanJump()
    {
        return status.IsGrounded && !status.IsDashing;
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
        bool canDash = CheckIfCanDash();

        if (canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    bool CheckIfCanDash()
    {
        return !status.OnDashCooldown && !status.isClimbing && (status.IsGrounded || status.CanDashInAir);
    }

    IEnumerator DashCoroutine()
    {
        status.OnDashCooldown = true;
        status.IsDashing = true;
        Direction direction = GetHorizontalDirection();

        ApplyImpulseForceToDirection(status.DashForce, direction);
        PlayAnimation("dash");
        StartCoroutine(dashDust.PlayEffect(transform));

        yield return new WaitForSeconds(DASH_DURATION);
        ClearHorizontalForce();
        StopAnimation("dash");
        status.IsDashing = false;

        yield return new WaitForSeconds(status.DashCooldownDuration);
        status.OnDashCooldown = false;
    }

    Direction GetHorizontalDirection()
    {
        return transform.localScale.x < 0 ? Direction.Left : Direction.Right;
    }

    void Attack()
    {
        bool canAttack = !normalAttack.OnCooldown;

        if (canAttack)
        {
            StartCoroutine(normalAttack.PlayEffect(transform));
        }
    }

    bool CheckIfIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, EXTRA_HEIGHT, groundLayer);

        return hit.collider != null;
    }

    void Climb(Direction direction)
    {
        
    }

    bool CheckIfIsTouchedRope()
    {
        
    }

    void Die()
    {
        bool isDead = transform.position.y < -40f;

        if (isDead)
        {
            TeleportToOrigin();
            // UIHealthController uIHealthController = new UIHealthController();
            // uIHealthController.TakeDamage(100f);
        }
    }

}