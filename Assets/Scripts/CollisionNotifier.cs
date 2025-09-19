using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class CollisionNotifier : MonoBehaviour
{
    private bool _hasCollided;

    public event Action<GameObject> Collide;

    private void OnEnable()
    {
        _hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;

        if (_hasCollided == false)
        {
            if (collider is MeshCollider)
            {
                Collide?.Invoke(gameObject);
                _hasCollided = true;
            }
        }
    }
}
