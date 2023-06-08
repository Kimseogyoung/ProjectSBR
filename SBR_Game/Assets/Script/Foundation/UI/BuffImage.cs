using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffImage : UI_Base
{
    private BuffBase _buff;

    public float CurDuration { get; set; }
    public float Duration { get; private set; }

    protected override void InitImp()
    {
        Bind<Image>(UI.BuffImage.ToString());
        Bind<Image>(UI.Duration.ToString());
    }

    public void Refresh()
    {
        //var buffDurationValue = _curDuration / _buff.GetDuration();
        var durationImage = Get<Image>(UI.Duration.ToString());
        durationImage.fillAmount = CurDuration / Duration;
        
        //_curDuration -= Time.fixedDeltaTime;
        //if(_curDuration < 0)
        //{
        //    _buff = null;
        //    _curDuration = 0;
        //}
    }

    public bool IsFinish()
    {
        return Get<Image>(UI.Duration.ToString()).fillAmount <= 0;
    }

    public void SetBuffImage(Sprite buffSprite, float duration, float curDuration)
    {
        Get<Image>(UI.BuffImage.ToString()).sprite = buffSprite;
        Duration = duration;
        CurDuration = curDuration;
    }

    enum UI
    {
        BuffImage,
        Duration
    }
}
