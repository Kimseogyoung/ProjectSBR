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
    private Transform _effectRoot;

    private System.Random _random;
    public DamageTextSystem()
    {
        _random = new System.Random();
        _effectRoot = new GameObject().transform;
        _effectRoot.name = "EffectRoot";
        EventQueue.AddEventListener<ShowDamageTextEvent>(EEventActionType.SHOW_DAMAGE_TEXT, CreateDamageText);
    }

    public void Destroy()
    {
        EventQueue.RemoveAllEventListener(EEventActionType.SHOW_DAMAGE_TEXT);
        GameObject.Destroy(_effectRoot);
    }

    public void Update()
    {
        for (int i = 0; i < _damageTextTransformList.Count; i++)
        {
            var obj = _damageTextTransformList.Dequeue();

            if (obj.FinishTime <= APP.InGame.GetGameTime())
            {
                GameObject.Destroy(obj.Transform.gameObject);
                continue;
            }

            obj.Transform.Translate(0, APP.InGame.GetGameTime() * 0.1f * Time.fixedDeltaTime, 0);
            _damageTextTransformList.Enqueue(obj);

        }
    }
    private void CreateDamageText(ShowDamageTextEvent evt)
    {
        evt.Pos += new Vector3(0, 0, 0.7f);
        var tmpObj = Util.Resource.Instantiate<TMP_Text>("Effect/DamageText", evt.Pos + evt.Dir * _random.Next(1000)/1000f, _effectRoot);
        tmpObj.SetText(evt.Damage.ToString());
        _damageTextTransformList.Enqueue(new DamageText { FinishTime = APP.InGame.GetGameTime() + 0.3f, Transform = tmpObj.gameObject.transform });
    }

    public class DamageText
    {
        public float FinishTime;
        public Transform Transform;
    }

}

