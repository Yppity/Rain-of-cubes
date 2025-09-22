using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ColorSetter))]

public class Cube : MonoBehaviour
{
    private bool _hasCollided;
    private Rigidbody _rigidbody;
    private ColorSetter _colorSetter;

    public event Action<Cube> Collided;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colorSetter = GetComponent<ColorSetter>();
    }

    private void OnEnable()
    {
        _hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false)
        {
            if (collision.gameObject.TryGetComponent<Platform>(out _))
            {
                Collided?.Invoke(this);
                _hasCollided = true;
            }
        }
    }

    public void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetEventColor()
    {
        _colorSetter.SetEventColor();
    }

    public void StartLifetime(float time, Action<Cube> callback)
    {
        StartCoroutine(LifetimeRoutine(time, callback));
    }

    private IEnumerator LifetimeRoutine(float time, Action<Cube> callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke(this);
    }
}
