using System;
using JetBrains.Annotations;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Direction> OnMovePressed;
    public event Action OnMoveReleased;
    public event Action OnJumpPressed;
    public event Action OnDashPressed;
    public event Action OnAttackPressed;
    public event Action OnClimbPressed;
    public event Action<float> OnPlayerHurt;

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

    void Update()
    {
        HandleMove();
        HandleJump();
        HandleDash();
        HandleAttack();
        HandleClimb();
        HandlePlayHurt();
    }

    void HandleMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            OnMovePressed?.Invoke(Direction.Left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            OnMovePressed?.Invoke(Direction.Right);
        }
        else
        {
            OnMoveReleased?.Invoke();
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnDashPressed?.Invoke();
        }
    }

    void HandleClimb()
    {
        bool playerTouchedRope = false;

        if (playerTouchedRope && Input.GetKeyDown(KeyCode.W))
        {

        }
        else if (playerTouchedRope && Input.GetKeyDown(KeyCode.S))
        {

        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnAttackPressed?.Invoke();
        }
    }

    void HandlePlayHurt()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            OnPlayerHurt?.Invoke(5f);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnPlayerHurt?.Invoke(45f);
        }
    }

}
