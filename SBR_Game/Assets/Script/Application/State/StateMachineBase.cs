using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


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

    public void SetCharacter(T character)
    {
        _character = character;
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
        dir = dir * _character.SPEED * Time.deltaTime;
        _transform.Translate( new Vector3(dir.x, 0, dir.y));
    }

    public void SetCharacterPos(Vector3 pos)
    {
        _transform.position = pos;
    }

    virtual protected void Init()
    {

    }
}