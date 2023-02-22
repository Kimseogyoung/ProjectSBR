using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
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

public class StateMachineBase<T> : MonoBehaviour where T : CharacterBase
{
    [SerializeField] protected T _character;
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
    }

    public void SetCharacter(T character, ECharacterType characterType)
    {
        _character = character;
        _character.SetCharacterType(characterType);
        _transform.position = _character.CurPos;
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
        currentState.OnEnterBase(_character);

    }

    //Character
    public void MoveCharacter(Vector2 dir)
    {
        _character.CurDir = new Vector3(dir.x, 0, dir.y);
        dir = dir * _character.SPEED * Time.deltaTime;
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
        FindTargetAndApplyDamage(new HitBox(EHitShape.Corn, _character.AttackRangeRadius, _character.CurDir, _character.AttackRangeAngle)
            , EHitType.ALONE
            , EAttack.ATK);   
    }

    private void FindTargetAndApplyDamage(HitBox hitBox, EHitType hitType, EAttack attackPowerType)
    {
        List<CharacterBase> targetList = new List<CharacterBase>();
        List<CharacterBase> enemyList = _characterList.GetEnemyList();
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (hitBox.CheckHit(_character.CurPos, enemyList[i].CurPos))
            {
                targetList.Add(enemyList[i]);
            }
        }

        if (targetList.Count == 0) return;//적이 없음

        switch (hitType)
        {
            case EHitType.ALONE:
                CharacterBase target = targetList[0];
                float distance = (target.CurPos - _character.CurPos).magnitude;
                for (int i=1; i< targetList.Count; i++)
                {
                    float newTargetDistance = (targetList[i].CurPos -_character.CurPos).magnitude;
                    if (distance > newTargetDistance)
                    {
                        target = targetList[i];
                        distance= newTargetDistance;
                    }
                }
                ApplyDamageToTarget(_character, target, attackPowerType);
                break;
            case EHitType.ALL:
                for(int i=0; i< targetList.Count; i++)
                {
                    ApplyDamageToTarget(_character, targetList[i], attackPowerType);
                }
                break;
            default:

                break;
        }
    }

    private void ApplyDamageToTarget(CharacterBase attacker, CharacterBase victim, EAttack atk, float multiplier = 1f)
    {
        GameLogger.Info("{0}이 {1}에게 맞음", victim.Name, attacker.Name);
        victim.ApplyDamage(_character.AccumulateDamage(attacker, victim, atk, multiplier));
    }

    virtual protected void Init()
    {

    }

    private void TestDrawCircle()
    {
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);
    }


    private ICharacterAccessible _characterList;
    public void SetCharacterAccessible(ICharacterAccessible characters) { _characterList = characters; }
}