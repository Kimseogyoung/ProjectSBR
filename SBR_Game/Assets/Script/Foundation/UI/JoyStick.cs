using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : UI_Panel, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool _isDragging = false;
    private float radius;
    private RectTransform _joyStickTransfrom;
    private Vector2 _currentClickPos;

    private void Awake()
    {
        radius = Bind<RectTransform>(UI.Pad.ToString()).rect.width * 0.3f;
        _joyStickTransfrom = Bind<RectTransform>(UI.JoyStick.ToString());

    }

    private void Update()
    {
        if (!_isDragging)
            return;

        SetJoyStickPos(_currentClickPos);
    }

    public void SetJoyStickPos(Vector2 clickPos)
    {
        Vector2 padPos = Get<RectTransform>(UI.Pad.ToString()).position;

        Vector2 dir = clickPos - padPos;
        
        if (dir.magnitude > radius)
        {
            dir = dir.normalized * radius;
        }

        _joyStickTransfrom.position = padPos + dir;
        Vector2 nomalizedDir = dir.normalized;

        APP.InputManager.InvokeMoveKeyAction(nomalizedDir);

    }

    public void OnDrag(PointerEventData eventData)
    {
        _currentClickPos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDrag(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetDrag(false);
    }

    private void SetDrag(bool isDraging)
    {
        _isDragging = isDraging;
        if (!_isDragging)
        {
            SetJoyStickPos(Get<RectTransform>(UI.Pad.ToString()).position);
        }
    }
    enum UI
    {
        JoyStick,
        Pad
    }
}
