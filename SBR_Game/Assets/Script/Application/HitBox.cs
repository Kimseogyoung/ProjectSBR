using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitShape
{
    Circle,//¿ø
    Corn,//¿ø»Ô
    Squre
}

public class HitBox
{
    private HitShape _hitShape;
    //private Vector2 _s;

    //Circle
    private float _radius;
    private Vector2 _centerPos;

    //
    private float _width;
    private float _height;

    public HitBox(HitShape hitShape, Vector2 centerPos, float radius)
    {
        _hitShape = hitShape;
        _centerPos = centerPos;
        _radius = radius;
    }

    public bool CheckHit(Vector2 attackerPos, Vector2 targetPos, Vector2 hitCenter)
    {
        switch (_hitShape)
        {
            case HitShape.Circle:
                if (Mathf.Pow(_radius, 2) >= (Mathf.Pow(hitCenter.x - targetPos.x, 2) + Mathf.Pow(hitCenter.x - hitCenter.y, 2)))
                {
                    return true;
                }
                break;
            case HitShape.Corn:
                break;
            case HitShape.Squre:
                break;
            default:
                return false;
        }

        return false;
        
    }
}
