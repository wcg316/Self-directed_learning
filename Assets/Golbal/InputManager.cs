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

    }
}
