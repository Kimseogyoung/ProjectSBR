using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DamageTextSystem
{
    private Queue<DamageText> _damageTextTransformList = new Queue<DamageText>();
    private Queue<DamageText> _healTextTransformList = new Queue<DamageText>();

    private ObjectPool<TMP_Text> _damageObjectPool;
    private ObjectPool<TMP_Text> _healTextPool;

    private Transform _effectRoot;

    private System.Random _random;

    public DamageTextSystem()
    {
        _random = new System.Random();
        _effectRoot = new GameObject().transform;
        _effectRoot.name = "EffectRoot";
;
        _damageObjectPool = new ObjectPool<TMP_Text>(15 , _effectRoot, Resources.Load<GameObject>("UI/Object/DamageText"));
        _healTextPool = new ObjectPool<TMP_Text>(15, _effectRoot, Resources.Load<GameObject>("UI/Object/HealText"));

        EventQueue.AddEventListener<ShowTextEvent>(EEventActionType.SHOW_DAMAGE_TEXT, CreateDamageText);
        EventQueue.AddEventListener<ShowTextEvent>(EEventActionType.SHOW_HEAL_TEXT, CreateHealText);
    }

    public void Destroy()
    {
        EventQueue.RemoveAllEventListener(EEventActionType.SHOW_DAMAGE_TEXT);

        _damageObjectPool.Destroy();
        GameObject.Destroy(_effectRoot);
    }

    public void Update()
    {
        for (int i = 0; i < _damageTextTransformList.Count; i++)
        {
            var obj = _damageTextTransformList.Dequeue();

            if (obj.FinishTime <= APP.InGame.GetGameTime())
            {
                _damageObjectPool.Enqueue(obj.Transform.gameObject);
                continue;
            }

            obj.Transform.Translate(0, APP.InGame.GetGameTime() * 0.1f * Time.fixedDeltaTime, 0);
            _damageTextTransformList.Enqueue(obj);

        }

        for (int i = 0; i < _healTextTransformList.Count; i++)
        {
            var obj = _healTextTransformList.Dequeue();

            if (obj.FinishTime <= APP.InGame.GetGameTime())
            {
                _healTextPool.Enqueue(obj.Transform.gameObject);
                continue;
            }

            obj.Transform.Translate(0, APP.InGame.GetGameTime() * 0.1f * Time.fixedDeltaTime, 0);
            _healTextTransformList.Enqueue(obj);

        }
    }

    private void CreateDamageText(ShowTextEvent evt)
    {
        var textObj = _damageObjectPool.Dequeue();

        evt.Pos += new Vector3(0, 0, 0.6f);
        textObj.gameObject.transform.position = evt.Pos + new Vector3(_random.Next(5) / 10f, _random.Next(5) / 10f, _random.Next(5) / 10f);
        textObj.component.SetText(evt.Value.ToString());
        _damageTextTransformList.Enqueue(new DamageText { FinishTime = APP.InGame.GetGameTime() + 0.3f, Transform = textObj.gameObject.transform });
    }

    private void CreateHealText(ShowTextEvent evt)
    {
        var textObj = _healTextPool.Dequeue();

        evt.Pos += new Vector3(0, 0, 0.6f);
        textObj.gameObject.transform.position = evt.Pos + new Vector3(_random.Next(5) / 10f, _random.Next(5) / 10f, _random.Next(5) / 10f);
        textObj.component.SetText(evt.Value.ToString());
        _healTextTransformList.Enqueue(new DamageText { FinishTime = APP.InGame.GetGameTime() + 0.3f, Transform = textObj.gameObject.transform });
    }

    public class DamageText
    {
        public float FinishTime;
        public Transform Transform;
    }

}

