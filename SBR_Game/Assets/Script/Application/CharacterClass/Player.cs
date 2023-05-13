using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Player : CharacterBase
{
    public Player(int characterId, int createNum) : base(characterId, ECharacterType.PLAYER, createNum)
    {
    }

    public void ControlActorMoving()
    {

    }

}
