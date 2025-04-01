using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    [SerializeField] private LayerMask droppableLayer;

    [ColorPalette]
    [SerializeField] private Color highlightColor = Color.green;

    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;
    private DraggableWorldSpace _currentDraggable;
    public DraggableWorldSpace CurrentDraggable { get => _currentDraggable; set => _currentDraggable = value;}
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    public void Highlight(){
        _spriteRenderer.color = highlightColor;
    }

    internal void NormalColor()
    {
        _spriteRenderer.color = _defaultColor;
    }
}
