using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : IManager, IManagerUpdatable
{
    private List<CharacterBase> _enemyList= new List<CharacterBase>();
    private List<CharacterBase> _heroList = new List<CharacterBase>();
    private Player player;

    public void Init()
    {
        player = (Player)Spawn(0);
        _enemyList.Add(Spawn(1));
        _heroList.Add(player);
    }

    public void StartManager()
    {
        for(int i=0; i<_enemyList.Count; i++)
        {
            //_enemyList[i].Set
        }
    }

    public void UpdateManager()
    {
     
    }

    public void FinishManager()
    {

    }

    public CharacterBase Spawn(int id)
    {
        GameObject characterObj;
        if (id == 0)
        {
            characterObj = Util.Resource.Instantiate(Path.CharacterDir + "Hero1");
            PlayerStateMachine stateMachine = Util.GameObj.GetOrAddComponent<PlayerStateMachine>(characterObj);
            Player player = new Player();
            stateMachine.SetCharacter(player);
            return player;
        }
        else
        {
            characterObj = Util.Resource.Instantiate(Path.CharacterDir + "Enemy1");
            CharacterStateMachine stateMachine = Util.GameObj.GetOrAddComponent<CharacterStateMachine>(characterObj);
            CharacterBase characterBase = new CharacterBase();
            stateMachine.SetCharacter(characterBase);
            return characterBase;
        }
    }
}
