using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillSystem
{
    private Queue<PushAction> _pushActionQueue = new Queue<PushAction>();

    public SkillSystem()
    {

    }

    public void UpdateSkill()
    {
        for(int i=0; i< _pushActionQueue.Count; i++)
        {
            PushAction pushAction = _pushActionQueue.Dequeue();
            if (pushAction.Character.IsDead())
                continue;

            Vector3 pushPos = pushAction.PushDir * pushAction.PushSpeed * Time.fixedDeltaTime;
            pushAction.Distance += pushPos.magnitude;
            pushAction.Character.TranslatePos(pushPos);

            if (pushAction.Distance >= pushAction.PushDistance)
                continue;

            _pushActionQueue.Enqueue(pushAction);

        }
    }

    public void AddPushAction(CharacterBase victim, float pushSpeed, float pushDistance, Vector3 pushDir)
    {
        _pushActionQueue.Enqueue(new PushAction(victim, pushSpeed, pushDistance, pushDir));
    }

    public class BufAction
    {
        public CharacterBase Character { get; private set; }

        public BufAction()
        {

        }
    }


    public class PushAction
    {
        public float Distance { get; set; } = 0;
        public CharacterBase Character { get; private set; }
        public float PushSpeed { get; private set; }
        public float PushDistance { get; private set; }
        public Vector3 PushDir { get; private set; }

        public PushAction(CharacterBase character, float pushSpeed, float pushDistance, Vector3 pushDir)
        {
            Character = character;
            PushSpeed = pushSpeed;
            PushDistance = pushDistance;
            PushDir = pushDir;
        }
    }
}

