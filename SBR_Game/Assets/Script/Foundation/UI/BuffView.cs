using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffView : UI_Base
{
    private RectTransform _curBuffImage;
    private int _count;
    private float _pos;
    private float _movePos;
    private bool _isScroll = false;

    protected override void InitImp()
    {
        _curBuffImage = Bind<RectTransform>(UI.BuffList.ToString());
        _pos = _curBuffImage.localPosition.x;
        _movePos = _curBuffImage.rect.xMax - _curBuffImage.rect.xMax / _count;
    }

    public void Refresh()
    {
        if (!_isScroll)
        {
            return;
        }

        _curBuffImage.localPosition = Vector2.Lerp(_curBuffImage.localPosition, new Vector2(_movePos, 0), Time.fixedDeltaTime * 5);

        if(Vector2.Distance(_curBuffImage.localPosition, new Vector2(_movePos, 0)) < 0.1f)
        {
            _isScroll = false;
        }
        
    }

    public void AddBuffImage()
    {

    }

    void Left()
    {
        if(_curBuffImage.rect.xMax - _curBuffImage.rect.xMax / _count == _movePos)
        {

        }
        else
        {
            _isScroll = true;
            _movePos = _pos + _curBuffImage.rect.width / _count;
            _pos = _movePos;
        }
    }

    enum UI
    {
        BuffList,
    }
}
