using UnityEngine;
using System.Collections;
using static UnityEngine.Random;
using Unity.Collections;
using Unity.Burst.CompilerServices;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destory(gameObject);
        }
    }

    // about health
    private float hp;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
        }
    }

    // about battle

    // about movement
    private bool isGrounded;
    public bool IsGrounded
    {
        get => isGrounded;
        set
        {
            isGrounded = value;
        }
    }
    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
        }
    }
    private float dashForce;
    public float DashForce
    {
        get => dashForce;
        set
        {
            dashForce = value;
        }
    }
    private bool isDashing;
    public bool IsDashing
    {
        get => isDashing;
        set
        {
            isDashing = value;
        }
    }
    private float jumpForce;
    public float JumpForce
    {
        get => jumpForce;
        set
        {
            jumpForce = value;
        }
    }

    // others
}