using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Util;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum EHitType
{
    ALONE,
    ALL
}

public enum EHitShape
{
    Circle,//��
    Corn,//����
    Squre
}

public class HitBox
{
    private EHitShape _hitShape;

    //Circle
    private float _radius;
    private Vector3 _centerPos;

    //Corn
    private Vector3 _dir;
    private float _angle;
    
    
    private float _width;
    private float _height;

    public HitBox(EHitShape hitShape, Vector3 centerPos, float radius)
    {
        _hitShape = hitShape;
        _centerPos = centerPos;
        _radius = radius;
    }

    public HitBox(EHitShape hitShape, float radius, Vector3 dir, float angle)
    {
        _hitShape = hitShape;
        _radius = radius;
        _dir = dir;
        _angle = angle;
    }


    public bool CheckHit(Vector3 attackerPos, Vector3 targetPos)
    {
        switch (_hitShape)
        {
            case EHitShape.Circle:
                GizmoHelper.PushDrawQueue(DrawCircle,2);
                return IsTargetInCircle(targetPos, _centerPos, _radius);
            case EHitShape.Corn:

                _centerPos = attackerPos;
                GizmoHelper.PushDrawQueue(DrawCorn,0.5f);
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
                break;

            case EHitShape.Squre:
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

    private void DrawCircle()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_centerPos, _radius);
    }

    private void DrawCorn()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(_centerPos, Vector3.up, _dir, _angle / 2, _radius);
        Handles.DrawSolidArc(_centerPos, Vector3.up, _dir, -_angle / 2, _radius);
    }
}
