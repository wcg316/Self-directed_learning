using UnityEngine;
using UnityEngine.UIElements.Experimental;

public enum ColliderShape
{
    Stand,
    RunRight,
    RunLeft
}

[System.Serializable]
public class NamedCollider
{
    public string name;
    public EdgeCollider2D collider;
    public bool Enabled
    {
        get => collider.enabled;
        set => collider.enabled = value;
    }
}

public class ColliderController : MonoBehaviour
{
    public NamedCollider[] colliders;

    public void InitializeColliders()
    {
        foreach (NamedCollider namedCollider in colliders)
        {
            namedCollider.collider.enabled = false;
        }
        EnableColliderByIndex((int)ColliderShape.Stand);
    }

    public void EnableColliderByIndex(int idx)
    {
        if (0 <= idx && idx < colliders.Length && colliders[idx] != null)
        {
            colliders[idx].Enabled = true;
            //Debug.Log("YEEEE " + colliders[idx].name);
        }
    }

    public void DisableColliderByIndex(int idx)
    {
        if (0 <= idx && idx < colliders.Length && colliders[idx] != null)
        {
            colliders[idx].Enabled = false;
            //Debug.Log("WRYYY " + colliders[idx].name);
        }
    }

    public void EnableCollider(Collider collider)
    {
        collider.enabled = true;
    }

    public void DisableCollider(Collider collider)
    {
        collider.enabled = false;
    }

    public EdgeCollider2D FindTheOnlyOneColliderEnabled()
    {
        foreach (NamedCollider namedCollider in colliders)
        {
            if (namedCollider.collider.enabled)
            {
                return namedCollider.collider;
            }
        }
        Debug.Log("Not found collider by FindTheOnlyOneColliderEnabled()");
        return new EdgeCollider2D();
    }

}