using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offget;

    void Start()
    {
        offget = new Vector3(0, -1, -100);
    }

    void Update()
    {
        if (target.position.x > -10 && target.position.y > -10)
        {
            transform.position = target.position + offget;
        }

    }
}
