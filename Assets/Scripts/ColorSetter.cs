using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class ColorSetter : MonoBehaviour
{
    private Color _defaultColor = Color.green;
    private Color _eventColor = Color.red;

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = _defaultColor;
    }

    public void SetEventColor()
    {
        GetComponent<Renderer>().material.color = _eventColor;
    }
}
