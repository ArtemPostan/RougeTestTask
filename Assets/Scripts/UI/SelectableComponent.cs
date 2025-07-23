using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SelectableComponent : MonoBehaviour
{
    public event Action<SelectableComponent> OnSelected;
    private SpriteRenderer _sr;
    private Color _original;
    public Color highlightColor = Color.yellow;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _original = _sr.color;
    }

    private void OnMouseDown()
    {
        OnSelected?.Invoke(this);
    }

    public void Highlight(bool on)
    {
        _sr.color = on ? highlightColor : _original;
    }
}