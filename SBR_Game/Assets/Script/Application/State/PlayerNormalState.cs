using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class playerNormalState : CharacterState<CharacterBase>
{
    protected override void OnEnter()
    {

    }

    protected override void OnExit()
    {

    }

    protected override void Update()
    {
        if (_character.IsDead())
        {
            _stateMachine.SetState(new DeadState());
            return;
        }
    }
}
