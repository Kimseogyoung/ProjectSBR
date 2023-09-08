using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffView : UI_Base
{

    private Character _character;
    private ObjectPool<BuffImage> _buffImagePool;


    protected override void InitImp()
    {
        _buffImagePool = new ObjectPool<BuffImage>(10, Bind<GameObject>(UI.BuffContent.ToString()).transform, Resources.Load<GameObject>("UI/Object/Buff"));
        for(int i=0; i<_buffImagePool.PoolCount(); i++)
        {
            var buff = _buffImagePool.Dequeue();
            buff.component.Init();
            _buffImagePool.Enqueue(buff.gameObject);
        }

    }

    protected override void OnDestroyed()
    {

        if (_character != null)
        {
            _character.OnAddBuff = null;
        }
        _character = null;
       

        _buffImagePool.Destroy();
    }

    public void AttachCharacter(Character character)
    {
        _character = character;
        _character.OnAddBuff = AddBuff;

        for (int i=0; i<_character.BuffList.Count; i++)
        {
            AddBuff(_character.BuffList[i]);
        }
    }

    public void AddBuff(BuffBase buff)
    {
        var buffImage = _buffImagePool.Dequeue();

        // TODO : buffImage.component.SetBuffImage(Resources.Load<Sprite>(buff.Proto.Name));

        buffImage.component.SetBuffImage(Resources.Load<Sprite>("Sprite/Nemo"), buff.GetDuration(), buff.GetCurDuration());
        LOG.I("ADD BUFF" + buff.Proto.Name + buff.GetDuration(), buff.GetCurDuration());
    }

    public void RemoveBuff(BuffImage buffImage)
    {
        LOG.I("Remove BuffImage");
        _buffImagePool.Enqueue(buffImage.gameObject);
    }

    public void Refresh()
    {
        if (_character == null || _character.IsDead())
        {
            return;
        }


        for (int i = 0; i < _buffImagePool.ActiveCount(); i++)
        {
            var buffImage = _buffImagePool.GetActiveList()[i];
            buffImage.component.CurDuration -= Time.fixedDeltaTime;
            if (buffImage.component.CurDuration < 0)
            {
                RemoveBuff(buffImage.component);
                return;
            }

            _buffImagePool.GetActiveList()[i].component.Refresh();
        }
        
    }

    enum UI
    {
        BuffContent
    }
}
