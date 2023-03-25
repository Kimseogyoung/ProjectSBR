using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterAnim
{
    NONE,
    IDLE,
    RUN,
    ATTACK,
    START,
    DIE
}

public class CharacterEventHandler : MonoBehaviour
{
    private Animator _animator;
    private Transform _transform;
    private Quaternion _quaternion;

    private Vector3 _rightDir;
    private Vector3 _leftDir;
    public delegate void CEventHandler();
    public event CEventHandler OnApplySkill;

    public bool _isMoving = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _transform = gameObject.transform;
        _quaternion =_transform.rotation;

        _leftDir = _quaternion.eulerAngles;
        _rightDir = new Vector3(-_leftDir.x, 180, _rightDir.z);

    }

    public void Move(Vector3 dir)
    {
        if(dir.x < 0)
            _transform.rotation = Quaternion.Euler(_leftDir);
        else if(dir.x > 0)
            _transform.rotation = Quaternion.Euler(_rightDir);
        _isMoving = true;
    }

    public void PlayAnim(string animName)
    {
        _animator.Play(animName, -1, 0f);
    }

    private void FixedUpdate()
    {
        _animator.SetBool("IsMoving", _isMoving);
        _isMoving = false;
        //종료 시점 체크
        /*
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("CurrentStateName") &&
     _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Current State Name 애니메이션 종료.");
        }
        */
    }

    private void OnApplyedSkill()
    {
        OnApplySkill?.Invoke();
    }
   
}
