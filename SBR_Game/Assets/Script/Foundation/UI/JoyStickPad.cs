using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickPad : UI_Base, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool _isDragging = false;
    private float radius;
    private RectTransform _joyStickTransfrom;
    private Vector2 _currentClickPos;


    protected override void InitImp()
    {
        radius = Bind<RectTransform>(UI.JoyStickPad.ToString()).rect.width * 0.3f;
        _joyStickTransfrom = Bind<RectTransform>(UI.JoyStick.ToString());

    }

    public void Refresh()
    {
        if (!_isDragging)
            return;

        SetJoyStickPos(_currentClickPos);
    }

    public void SetJoyStickPos(Vector2 clickPos)
    {
        Vector2 padPos = Get<RectTransform>(UI.JoyStickPad.ToString()).position;

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
            _joyStickTransfrom.position = Get<RectTransform>(UI.JoyStickPad.ToString()).position;
        }
    }
    enum UI
    {
        JoyStick,
        JoyStickPad
    }
}
