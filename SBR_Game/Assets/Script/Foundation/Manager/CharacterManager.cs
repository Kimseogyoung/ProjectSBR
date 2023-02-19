using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICharacterAccessible
{
    public List<CharacterBase> GetEnemyList();
    public List<CharacterBase> GetHeroList();
    public List<CharacterBase> GetAllCharacterList();

}

public class CharacterManager : IManager, IManagerUpdatable, ICharacterAccessible
{
    private List<CharacterBase> _enemyList = new List<CharacterBase>();
    private List<CharacterBase> _heroList = new List<CharacterBase>();
    private Player player;

    public void Init()
    {
        player = (Player)Spawn(1001);
        Spawn(1010);
    }

    public void StartManager()
    {
        for (int i = 0; i < _enemyList.Count; i++)
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
        CharacterBase character;
        GameObject characterObj;

        if (id == 1001)//플레이어 캐릭터라면 (수정)
        {
            characterObj = Util.Resource.Instantiate(Path.CharacterDir + "Hero1");
            PlayerStateMachine stateMachine = Util.GameObj.GetOrAddComponent<PlayerStateMachine>(characterObj);
            stateMachine.SetCharacterAccessible(this);
            character = new Player();
            stateMachine.SetCharacter((Player)character);
        }
        else
        {
            characterObj = Util.Resource.Instantiate(Path.CharacterDir + "Enemy1");
            CharacterStateMachine stateMachine = Util.GameObj.GetOrAddComponent<CharacterStateMachine>(characterObj);
            stateMachine.SetCharacterAccessible(this);
            character = new CharacterBase();
            stateMachine.SetCharacter(character);

        }

        if (id < 1010)
        {
            _heroList.Add(character);
            //플레이어 캐릭터라면
        }
        else
        {
            _enemyList.Add(character);

        }

        return character;
    }

    public List<CharacterBase> GetEnemyList() { return _enemyList; }
    public List<CharacterBase> GetHeroList() { return _heroList; }
    public List<CharacterBase> GetAllCharacterList() { return (List<CharacterBase>)_enemyList.Concat(_heroList); }
}
