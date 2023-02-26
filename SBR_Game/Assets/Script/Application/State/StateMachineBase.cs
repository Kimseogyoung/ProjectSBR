using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum ECharacterType
{
    None = 0,
    Player,
    Boss,
    Zzol
}

public class StateMachineBase : MonoBehaviour 
{
    [SerializeField] protected CharacterBase _character;
    private CharacterState<CharacterBase> currentState;

    private Transform _transform;//현재 캐릭터 위치

    private void Awake()
    {
        _transform = gameObject.transform;
        //상태 전환 흐름 설정
        Init();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (currentState == null) return;
        currentState.UpdateBase();
        _character.GetSkill(EInputAction.SKILL1).UpdateSkill();
        _character.GetSkill(EInputAction.SKILL2).UpdateSkill();
        _character.GetSkill(EInputAction.SKILL3).UpdateSkill();
        _character.GetSkill(EInputAction.ULT_SKILL).UpdateSkill();
    }

    public void SetCharacter(CharacterBase character, ECharacterType characterType)
    {
        _character = character;
        _character.SetCharacterType(characterType);
        _transform.position = _character.CurPos;
        SetState(new IdleState());
    }

    public void SetState(CharacterState<CharacterBase> nextState)
    {
        if (currentState != null)
        {
            // 기존의 상태가 존재했다면 OnExit()호출
            currentState.OnExitBase();

        }

        // 다음state 시작
        currentState = nextState;
        currentState.OnEnterBase(_character, this);

    }

    //Character
    public void MoveCharacter(Vector2 dir)
    {
        _character.CurDir = new Vector3(dir.x, 0, dir.y);
        dir = dir * _character.SPD * Time.deltaTime;
        _character.CurPos += new Vector3(dir.x, 0, dir.y);
        SetCharacterPos();

        GizmoHelper.PushDrawQueue(TestDrawCircle);
    }

    public void SetCharacterPos()
    {
        _transform.position = _character.CurPos;
    }

    public void Attack()
    {
        APP.Characters.FindTargetAndApplyDamage(_character, new HitBox(EHitShape.Corn, _character.AttackRangeRadius, _character.CurDir, _character.AttackRangeAngle)
            , EHitType.ALONE
            , EAttack.ATK);   
    }

    public void UseSkill1() => UseSkill(EInputAction.SKILL1);
    public void UseSkill2() => UseSkill(EInputAction.SKILL2); 
    public void UseSkill3() => UseSkill(EInputAction.SKILL3);
    public void UseUltSkill() => UseSkill(EInputAction.ULT_SKILL);

    private void UseSkill(EInputAction inputAction)
    {
        SkillBase skill= _character.GetSkill(inputAction);
        if(skill != null)
        {
            if (_character.GetSkill(inputAction).TryUseSkill())
            {
                GameLogger.Info("{0} 사용 성공", inputAction);
                //스킬 사용 성공
                return;
            }
        }
        //실패
    }

    virtual protected void Init()
    {

    }

    private void TestDrawCircle()
    {
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);
    }

}