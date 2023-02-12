using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    private List<IManagerUpdatable> _managerUpdatables = new List<IManagerUpdatable>();
    private List<IManager> _managers = new List<IManager>();

    private DataManager _dataManager;
    private SceneManager _sceneManager;
    // Start is called before the first frame update

    private void Awake()
    {
        GameLogger.Info("GameManager Awake");

        DontDestroyOnLoad(gameObject);

        _dataManager = new DataManager();
        AddManager(_dataManager);

        _sceneManager = new SceneManager();
        AddManager(_sceneManager, true);

        foreach (IManager manager in _managers)
        {
            manager.Init();
        }

    }

    void Start()
    {
        foreach(IManager manager in _managers)
        {
            manager.PrepareManager();
        }

        foreach (IManager manager in _managers)
        {
            manager.StartManager();
        }
    }

    // Update is called once per frame
    void Update()
    {

        for(int i=0; i<_managerUpdatables.Count; i++)
            _managerUpdatables[i].UpdateManager();
        
    }

    private void AddManager<T>(T manager, bool isUpdatable = false) where T :IManager
    {
        _managers.Add(manager);
        if (isUpdatable)
            _managerUpdatables.Add((IManagerUpdatable)manager);
    }

}
