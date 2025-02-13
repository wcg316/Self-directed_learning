using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target = GameObject.FindWithTag("Player").transform;
        offget = new Vector3(0, -1, -100);
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.x > -10 && target.position.y > -10)
        {
            transform.position = target.position + offget;
        }

    }
}
