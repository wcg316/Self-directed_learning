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
public class AttackType
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
    public AttackType(GameObject attackEffect, AudioClip attackSound,
                      float distanceX, float distanceY, int b_angle, int o_angle,
                      float attackDuration, float attackCooldown)
    {
        effect = attackEffect ?? effect;
        sound = attackSound ?? sound;
        this.distanceX = distanceX;
        this.distanceY = distanceY;
        baseAngle = b_angle;
        offsetAngle = o_angle;
        duration = attackDuration;
        cooldownDuration = attackCooldown;
        onCooldown = false;

        if (effect != null)
        {
            audioSource = effect.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = effect.AddComponent<AudioSource>();

            effect.SetActive(false);
        }

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
    public AudioClip normalAttackSound;
    public LayerMask groundLayer;
    private const float EXTRA_HEIGHT = 2.75f;
    public bool isGrounded = true;
    public float moveSpeed;
    public float sprintForce;
    private const float SPRINT_DURATION = 0.15f;
    private bool isSprinting = false;
    public float sprintCooldownDuration;
    public bool canSprintInAir = false;
    public bool onSprintCooldown = false;
    public float jumpForce;
    public bool playingFootstepSound;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GameObject normalAttackSlash;
    public bool onAttackCooldown = false;
    public AttackType normalAttack;

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
        colliderController.InitializeColliders();
    }

    void Update()
    {
        ForTesting();

        Variable();
        Move();
        Jump();
        Sprint();
        Attack();
    }

    void ForTesting()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            body.linearVelocity = Vector2.zero;
            transform.position = new Vector2(0f, -0.45f);
        }
        Debug.DrawRay(transform.position, Vector2.down * EXTRA_HEIGHT, Color.red);
    }


    void Variable()
    {
        isGrounded = CheckIfIsGrounded();
        moveSpeed = 10f;
        sprintForce = 100f;
        jumpForce = 30f;
        sprintCooldownDuration = 0.5f;
    }

    void Move()
    {
        bool canMove = !isSprinting;
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
        AdjustColliderWhileRunning(direction);
        MoveForwardWithSpeed(
            moveSpeed * Time.deltaTime * GetHorizontalDirectionMultiplier(direction)
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

    int GetHorizontalDirectionMultiplier(Direction direction)
    {
        return direction switch
        {
            Direction.Right => 1,
            Direction.Left => -1,
            _ => 0
        };
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
        return isGrounded && !isSprinting;
    }

    void ClearVerticalForce()
    {
        body.linearVelocityY = 0f;
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

    /* 舊版衝刺
    void Sprint()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Direction direction = GetDirection();
            PlayAnimation("sprint");
            MoveForwardWithSpeed(
                moveSpeed * Time.deltaTime * sprintAccelerate * GetMovingDirectionMultiplier(direction)
            );
        }
        else
        {
            StopAnimation("sprint");
        }
    }*/

    void Sprint()
    {
        bool playerCanSprint = CheckIfPlayerCanSprint();
        bool playerSprinted = Input.GetKeyDown(KeyCode.K) && playerCanSprint;

        if (playerSprinted)
        {
            StartCoroutine(SprintCoroutine());
        }
    }

    bool CheckIfPlayerCanSprint()
    {
        return !onSprintCooldown && (isGrounded || canSprintInAir);
    }

    IEnumerator SprintCoroutine()
    {
        onSprintCooldown = true;
        isSprinting = true;
        Direction direction = GetHorizontalDirection();

        ApplyImpulseForceToDirection(sprintForce, direction);
        PlayAnimation("sprint");

        yield return new WaitForSeconds(SPRINT_DURATION);
        body.linearVelocity = new Vector2(0f, body.linearVelocityY);
        StopAnimation("sprint");
        isSprinting = false;

        yield return new WaitForSeconds(sprintCooldownDuration);
        onSprintCooldown = false;
    }

    Direction GetHorizontalDirection()
    {
        return spriteRenderer.flipX ? Direction.Left : Direction.Right;
    }

    void Attack()
    {
        bool playerAttacked = Input.GetKeyDown(KeyCode.J) && !normalAttack.OnCooldown;
        Direction direction = GetHorizontalDirection();

        if (playerAttacked)
        {
            StartCoroutine(AttackCoroutine(normalAttack, direction));
        }
    }

    IEnumerator AttackCoroutine(AttackType attackType, Direction direction)
    {
        attackType.SetHorizontalDirectionMultiplier(GetHorizontalDirectionMultiplier(direction));

        attackType.SetPositionFrom(transform);
        attackType.SetAngle();
        ActivateAttackEffect(attackType.Effect);
        attackType.PlaySound();
        attackType.StartCooldown();

        yield return new WaitForSeconds(attackType.Duration);
        HideAttackEffect(attackType.Effect);

        yield return new WaitForSeconds(attackType.CooldownDuration);
        attackType.EndCooldown();
    }

    void ActivateAttackEffect(GameObject effect)
    {
        effect.SetActive(true);
    }

    void HideAttackEffect(GameObject effect)
    {
        effect.SetActive(false);
    }

    bool CheckIfIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, EXTRA_HEIGHT, groundLayer);

        return hit.collider != null;
    }

}