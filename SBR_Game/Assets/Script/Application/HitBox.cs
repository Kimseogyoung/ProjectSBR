using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using Util;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class HitBox
{
    private EHitShapeType _hitShape;

    //Circle
    private float _radius;
    private Vector3 _centerPos;


    //Corn
    private Vector3 _dir;
    private float _angle;
    
    
    private float _width;
    private float _height;

    private Quaternion _rotation;

    public Vector3 CenterPos;


    //Circle
    public HitBox(EHitShapeType hitShape, Vector3 centerPos, float radius)
    {
        _hitShape = hitShape;
        _centerPos = centerPos;
        _radius = radius;
    }

    //Corn
    public HitBox(EHitShapeType hitShape, float radius, Vector3 dir, float angle)
    {
        _hitShape = hitShape;
        _radius = radius;
        _dir = dir;
        _angle = angle;
    }

    //Squere
    public HitBox(EHitShapeType hitShape, Vector3 centerPos, Vector3 dir, float width, float height)
    {
        _hitShape = hitShape;
        _dir = dir;
        _width = width;
        _height = height;
        _centerPos = centerPos;
    }

    public void SetDirPos(Vector3 dir, Vector3 centerPos)
    {
        _dir = dir;
        _centerPos= centerPos;
    }

    public bool CheckHit(Vector3 attackerPos, Vector3 targetPos)
    {
        switch (_hitShape)
        {
            case EHitShapeType.CIRCLE:
                GizmoHelper.PushDrawQueue(DrawCircle,0.1f);
                return IsTargetInCircle(targetPos, _centerPos, _radius);
            case EHitShapeType.CORN:

                _centerPos = attackerPos;
                GizmoHelper.PushDrawQueue(DrawCorn,0.1f);
                if (!IsTargetInCircle(targetPos, _centerPos, _radius)) return false;

                // 'Ÿ��-�� ����'�� '�� ���� ����'�� ����
                float dot = Vector3.Dot((targetPos- _centerPos).normalized, _dir);
                // �� ���� ��� ���� �����̹Ƿ� ���� ����� cos�� ���� ���ؼ� theta�� ����
                float theta = Mathf.Acos(dot);

                // angleRange�� ���ϱ� ���� degree�� ��ȯ
                float degree = Mathf.Rad2Deg * theta;

                // �þ߰� �Ǻ�
                if (degree <= _angle / 2f)
                    return true;
                else
                    return false;

            case EHitShapeType.SQURE:
               
                // LookRotation()�� ����Ͽ� targetDir�� �������� ȸ���ϴ� ���ʹϾ� �� ���ϱ�
                _rotation = Quaternion.LookRotation(_dir);
                // ���ʹϾ� ���� �����̼� ������ ��ȯ�Ͽ� y �� ���� ��(���Ϸ� ��) ��ȯ
                float yRotation = _rotation.eulerAngles.y;
                GizmoHelper.PushDrawQueue(DrawSqure, 0.1f);
                if (IsTargetInRect(targetPos, _centerPos, new Vector3(_width, 1, _height), yRotation)) return true;
                break;
            default:
                return false;
        }

        return false;
        
    }

    private bool IsTargetInCircle(Vector3 targetPos, Vector3 hitCenter, float radius)
    {
        float distance = (targetPos - hitCenter).magnitude;
        if (distance > radius) return false;
        return true;
    }
    private bool IsTargetInRect(Vector3 targetPos, Vector3 rectCenter, Vector3 rectSize, float rectAngle)
    {
        // ���簢�� ���� ��ǥ��� ��ȯ
        Vector3 localPos = Quaternion.Euler(0f, -rectAngle, 0f) * (targetPos - rectCenter);

        // ���簢�� ���� ��ǥ�迡�� x, y ��ǥ ����
        float x = Mathf.Abs(localPos.x);
        float z = Mathf.Abs(localPos.z);

        // ���簢�� ���� ��ǥ�迡���� half width, half height ���
        float halfWidth = rectSize.x / 2f;
        float halfHeight = rectSize.z / 2f;
        // ���簢�� ���� ��ǥ�迡���� x, y ��ǥ�� half width, half height ���� ������ ���簢�� ���ο� ��ġ��
        return x < halfWidth && z < halfHeight;
    }

    private void DrawCircle()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_centerPos, _radius);
    }
    private void DrawSqure()
    {
        Gizmos.color = Color.red;
        // ť�� �׸���
        Gizmos.DrawLine(_centerPos - _dir * _height / 2, _centerPos + _dir * _height / 2);
        Gizmos.DrawLine(_centerPos - _dir * _height / 2, _centerPos + _dir * _height/2);
    }

    private void DrawCorn()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(_centerPos, Vector3.up, _dir, _angle / 2, _radius);
        Handles.DrawSolidArc(_centerPos, Vector3.up, _dir, -_angle / 2, _radius);
    }
}
