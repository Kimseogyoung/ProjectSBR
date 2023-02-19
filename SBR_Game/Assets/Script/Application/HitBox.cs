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
    Circle,//원
    Corn,//원뿔
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

                // '타겟-나 벡터'와 '내 정면 벡터'를 내적
                float dot = Vector3.Dot((targetPos- _centerPos).normalized, _dir);
                // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
                float theta = Mathf.Acos(dot);

                // angleRange와 비교하기 위해 degree로 변환
                float degree = Mathf.Rad2Deg * theta;

                // 시야각 판별
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
