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

            case EHitShapeType.SQURE:
               
                // LookRotation()을 사용하여 targetDir의 방향으로 회전하는 쿼터니언 값 구하기
                _rotation = Quaternion.LookRotation(_dir);
                // 쿼터니언 값을 로테이션 값으로 변환하여 y 축 방향 값(오일러 각) 반환
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
        // 직사각형 내부 좌표계로 변환
        Vector3 localPos = Quaternion.Euler(0f, -rectAngle, 0f) * (targetPos - rectCenter);

        // 직사각형 내부 좌표계에서 x, y 좌표 추출
        float x = Mathf.Abs(localPos.x);
        float z = Mathf.Abs(localPos.z);

        // 직사각형 내부 좌표계에서의 half width, half height 계산
        float halfWidth = rectSize.x / 2f;
        float halfHeight = rectSize.z / 2f;
        // 직사각형 내부 좌표계에서의 x, y 좌표가 half width, half height 보다 작으면 직사각형 내부에 위치함
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
        // 큐브 그리기
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
