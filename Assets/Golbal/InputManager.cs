using System;
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
        if (Input.GetKeyDown(KeyCode.W))
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

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnAttackPressed?.Invoke();
        }
    }

}
