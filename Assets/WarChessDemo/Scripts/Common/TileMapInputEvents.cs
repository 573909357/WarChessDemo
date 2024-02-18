using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMapInputEvents : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler,IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Type enable;
    [SerializeField] public Type frameData;

    [NonSerialized] public Action<PointerEventData, TileMapInputEvents> onPointerClick;
    [NonSerialized] public Action<PointerEventData, TileMapInputEvents> onPointerUp;
    [NonSerialized] public Action<PointerEventData, TileMapInputEvents> onPointerDown;
    [NonSerialized] public Action<PointerEventData, TileMapInputEvents> onPointerEnter;
    [NonSerialized] public Action<PointerEventData, TileMapInputEvents> onPointerExit;
    [NonSerialized] public Action<PointerEventData, TileMapInputEvents> onPointerMove;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!enable.HasFlag(Type.Click))
            return;
        frameData |= Type.Click;
        onPointerClick?.Invoke(eventData, this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!enable.HasFlag(Type.Up))
            return;
        frameData |= Type.Up;
        onPointerUp?.Invoke(eventData, this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!enable.HasFlag(Type.Down))
            return;
        frameData |= Type.Down;
        onPointerDown?.Invoke(eventData, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enable.HasFlag(Type.Enter))
            return;
        frameData |= Type.Enter;
        onPointerEnter?.Invoke(eventData, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enable.HasFlag(Type.Exit))
            return;
        frameData |= Type.Exit;
        onPointerExit?.Invoke(eventData, this);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!enable.HasFlag(Type.Move))
            return;
        frameData |= Type.Move;
        onPointerMove?.Invoke(eventData, this);
    }

    private void LateUpdate()
    {
        frameData = Type.None;
    }

    [Flags]
    public enum Type
    {
        None = 0,
        Click = 1 << 0,
        Up = 1 << 1,
        Down = 1 << 2,
        Enter = 1 << 3,
        Exit = 1 << 4,
        Move = 1 << 5,
        All = ~None,
    }
}