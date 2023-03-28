using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MovingSkill : SkillBase
{
    private bool _isMoving = false;
    protected override void UpdateSkill()
    {
        if (!_isMoving)
            return;

        _character.CurPos += _firstSkillDir * _skillProto.Speed * Time.fixedDeltaTime;
        if((_firstSkillPos - _character.CurPos).magnitude > _skillProto.Range)
        {
            _isMoving = false;
        }

    }

    protected override void ApplySkill()
    {
        _isMoving = true;
    }
}
