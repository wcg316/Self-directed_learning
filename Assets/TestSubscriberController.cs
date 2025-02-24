using System;
using UnityEngine;

public class TestSubscriberController : MonoBehaviour
{


    void Start()
    {
        InputManager.Instance.OnMove += OnMove;
    }

    void OnMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                Debug.Log("Move Up");
                break;
            case Direction.Down:
                Debug.Log("Move Down");
                break;
            case Direction.Left:
                Debug.Log("Move Left");
                break;
            case Direction.Right:
                Debug.Log("Move Right");
                break;
        }
    }
}
