using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : UI_Panel
{
    [SerializeField] private Character _character;
    private Vector3 _offset = new Vector3(0,150,0);
    public int CharacterCreateNum { get { return _character== null ? 0: _character.CreateNum; } }

    protected override void InitImp()
    {
        Bind<Slider>(UI.HpSlider.ToString());
    }

    protected override void OnDestroyed()
    {
        base.OnDestroyed();
        _character = null;
    }

    public void SetCharacter(Character character)
    {
        _character = character;
        RefreshHp();
    }
    
    public bool IsFar()
    {
        // 체력바 위치 설정 // 캐릭터의 월드 좌표를 스크린 좌표로 변환
        Vector2 screenPos = Camera.main.WorldToScreenPoint(_character.CurPos);

        var radius = Mathf.Max(Screen.width, Screen.height);
        var screenCenterPos = new Vector2(Screen.width / 2f, Screen.height / 2f);

        return (screenPos - screenCenterPos).magnitude > radius;
    }

    public void Refresh()
    {
        if(_character == null)
        {
            return;
        }

        var hpSlider = Get<Slider>(UI.HpSlider.ToString());

        RefreshPos(hpSlider);
        RefreshHp(hpSlider);
    }

    private void RefreshPos(Slider hpSlider = null)
    {
        if (hpSlider == null)
        {
            hpSlider = Get<Slider>(UI.HpSlider.ToString());
        }
       
        // 체력바 위치 설정 // 캐릭터의 월드 좌표를 스크린 좌표로 변환
        Vector2 screenPos = Camera.main.WorldToScreenPoint(_character.CurPos);
        hpSlider.transform.position = new Vector3(screenPos.x, screenPos.y, 0) + _offset;
    }

    private void RefreshHp(Slider hpSlider = null)
    {
        if(hpSlider == null)
        {
            hpSlider = Get<Slider>(UI.HpSlider.ToString());
        }

        hpSlider.maxValue = _character.HP.FullValue;
        hpSlider.value = _character.HP.Value;
    }


    enum UI{
        HpSlider,
    }
}
