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

    public event Action<Direction> OnMove;

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnMove?.Invoke(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            OnMove?.Invoke(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            OnMove?.Invoke(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnMove?.Invoke(Direction.Right);
        }
    }
}
