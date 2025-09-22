using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class ColorSetter : MonoBehaviour
{
    private Renderer _renderer;
    private Color _defaultColor = Color.green;
    private Color _eventColor = Color.red;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _renderer.material.color = _defaultColor;
    }

    public void SetEventColor()
    {
        _renderer.material.color = _eventColor;
    }
}
