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
    [SerializeField] private T _character;
    private CharacterState<CharacterBase> currentState;

    private Transform _transform;//���� ĳ���� ��ġ

    private void Awake()
    {
        _transform = gameObject.transform;
        //���� ��ȯ �帧 ����
        Init();
    }

    private void Start()
    {
        SetState(new IdleState());
    }

    private void Update()
    {
        currentState.UpdateBase();
    }


    public void SetState(CharacterState<CharacterBase> nextState)
    {
        if (currentState != null)
        {
            // ������ ���°� �����ߴٸ� OnExit()ȣ��
            currentState.OnExitBase();

        }

        // ����state ����
        currentState = nextState;
        currentState.OnEnterBase(_character);

    }

    

    //Character
    public void MoveCharacter(Vector2 dir)
    {
        _transform.Translate( dir * _character.SPEED * Time.deltaTime);
    }

    public void SetCharacterPos(Vector2 pos)
    {
        _transform.position = pos;
    }

    virtual protected void Init()
    {

    }
}