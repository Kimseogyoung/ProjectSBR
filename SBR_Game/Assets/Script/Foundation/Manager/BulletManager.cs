using System;
using System.Collections.Generic;
using UnityEngine;
using static BulletManager;

public interface IBullet
{
    public void InstantiateBullet(Action<Vector3, Character> onFoundAction, Character owner, string bulletPrefab, Vector3 dir,
        float speed, float maximumDistance, ECharacterTeamType teamType, Character target = null);
}

public class BulletManager : IManager, IManagerUpdatable, IBullet
{
    private Transform _parent;
    private List<Bullet> _bulletList = new List<Bullet>();
    private Queue<Bullet> _removedBulletQueue = new Queue<Bullet>();

    private const float _bulletTime = 5; //유지시간

    public void InstantiateBullet(Action<Vector3, Character> onFoundAction, Character owner, string bulletPrefab, Vector3 dir, 
        float speed, float maximumDistance, ECharacterTeamType teamType, Character target = null)
    {
        GameObject bulletObj = UTIL.Instantiate(bulletPrefab);
        bulletObj.transform.SetParent(_parent);
        bulletObj.transform.position = owner.CurPos;
        

        _bulletList.Add(new Bullet(onFoundAction, owner, target, bulletObj.transform, dir, speed, maximumDistance, teamType));
    }

    public void FinishManager()
    {
    }

    public void Init()
    {
        APP.Bullet = this;
        _parent = new GameObject("BulletParent").transform;

    }

    public void Pause(bool IsPause)
    {
       
    }

    public void StartManager()
    {

    }

    public void UpdateManager()
    {
        for(int i=0; i<_bulletList.Count; i++)
        {
            if ((_bulletList[i].StartPosition - _bulletList[i].BulletTransform.position).magnitude > _bulletList[i].MaximumDistance)
            {
                _removedBulletQueue.Enqueue(_bulletList[i]);
                continue;
            }
            MoveBullet(_bulletList[i]);
        }

        while(_removedBulletQueue.Count > 0)
        {
            Bullet bullet = _removedBulletQueue.Dequeue();
            _bulletList.Remove(bullet);
            GameObject.Destroy(bullet.BulletTransform.gameObject);
        }
    }
    public void UpdatePausedManager()
    {
    }

    private void MoveBullet(Bullet bullet)
    {
        Vector3 normalizedDir;
        if (bullet.Target != null)
        {// 타겟이 있을 때
            Vector3 dir = bullet.Target.CurPos - bullet.BulletTransform.position;
            normalizedDir = dir.normalized;
            bullet.BulletTransform.Translate(normalizedDir * bullet.Speed * Time.fixedDeltaTime);

            if (dir.magnitude <= bullet.BulletHalfSize)
            {
                RemoveBullet(bullet, bullet.Target);
            }

        }
        else
        {// 타겟이 없을때
            normalizedDir = bullet.Dir;
            bullet.BulletTransform.Translate(normalizedDir * bullet.Speed * Time.fixedDeltaTime);


            // 충돌 검사
            RaycastHit hit;
            Debug.DrawRay(bullet.BulletTransform.position, normalizedDir, Color.red, 0.1f);
            if (Physics.Raycast(bullet.BulletTransform.position, normalizedDir, out hit, bullet.BulletHalfSize, bullet.LayerMask))
            {
                Character victim = hit.collider.GetComponent<StateMachineBase>().GetCharacter();
                RemoveBullet(bullet, victim);
            }

        }
        
    }

    private void RemoveBullet(Bullet bullet, Character victim)
    {
        _removedBulletQueue.Enqueue(bullet);
        bullet.OnFoundAction(bullet.BulletTransform.position, victim); 
    }

    public class Bullet
    {
        public Character Owner { get; private set; }
        public Character Target { get; private set; }
        public Transform BulletTransform { get; private set; }
        public float BulletHalfSize { get; private set; }
        public Vector3 Dir { get; private set; }
        public float Speed { get; private set; }
        public LayerMask LayerMask { get; private set; }
        public Action<Vector3, Character> OnFoundAction { get; private set; }
        public float MaximumDistance { get; private set; }
        public Vector3 StartPosition { get; private set; }

        public Bullet(Action<Vector3, Character> onFoundAction, Character owner, Character target, Transform bulletTransform, Vector3 dir, float speed, float maximumDistance,  ECharacterTeamType targetTeamType)
        {
            OnFoundAction= onFoundAction;
            Owner = owner;
            Target = target;
            BulletTransform = bulletTransform;
            Dir = dir;
            Speed = speed;
            LayerMask = targetTeamType == ECharacterTeamType.ENEMY ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Hero");
            BulletHalfSize = (bulletTransform.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.y) / 2;
            MaximumDistance= maximumDistance;

            StartPosition = BulletTransform.position;
        }
    }
}

