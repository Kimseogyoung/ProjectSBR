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

    private Transform _transform;//���� ĳ���� ��ġ

    private void Awake()
    {
        _transform = gameObject.transform;
        //���� ��ȯ �帧 ����
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
        _transform.position = _character.CurPos;
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
        dir = dir * _character.SPEED * Time.deltaTime;
        _character.CurPos += new Vector3(dir.x, 0, dir.y);
        SetCharacterPos();

        GizmoHelper.PushDrawQueue(TestDrawCircle);
    }

    public void SetCharacterPos()
    {
        _transform.position = _character.CurPos;

    }

    virtual protected void Init()
    {

    }
    private void TestDrawCircle()
    {
        Gizmos.DrawLine(transform.position, transform.forward + transform.position);
    }
}