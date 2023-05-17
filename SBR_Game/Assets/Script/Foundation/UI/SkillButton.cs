using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillButton : UI_Base
{
    private SkillBase _skill;

    private float _curCoolTIme = 0;
    private float _fullCoolTime = 0;

    private Image _coolDownImage;
	private Button _btn;

    protected override void InitImp()
    {
        _btn = GetComponent<Button>();
        _coolDownImage = Bind<Image>(UI.CoolDown.ToString());

        _btn.onClick.AddListener(OnClick);
    }

    protected override void OnDestroyed()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void SetSkill(SkillBase skill)
    {
        _skill = skill;
    }

    public void Refresh()
    {
        if(_skill == null)
        {
            return;
        }

        if (_skill.CanUseSkill())
        {
            _coolDownImage.fillAmount = 0;
            return;
        }

        if(_curCoolTIme < 0)
        {
            return;
        }

        _curCoolTIme = Math.Max(0, _curCoolTIme - Time.fixedDeltaTime);
        _coolDownImage.fillAmount = _curCoolTIme / _fullCoolTime;
    }

    public void OnClick()
    {
        if (!_skill.CanUseSkill()) 
        {
            GameLogger.Info($"Can Not Use Skill {_skill.Prt.Name}");
            return;
        }

        APP.InputManager.InvokeKeyAction(_skill.MatchedInputAction);

        _curCoolTIme = _skill.FullCoolTime;
        _fullCoolTime = _skill.FullCoolTime;

        Refresh();
    }

    enum UI
    {
        CoolDown = 0,
    }

}
