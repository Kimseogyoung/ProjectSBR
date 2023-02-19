using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    private List<IManagerUpdatable> _managerUpdatables = new List<IManagerUpdatable>();
    private List<IManager> _managers = new List<IManager>();

    private DataManager _dataManager;
    private SceneManager _sceneManager;
    private InputManager _inputManager;

    private bool _isStopped = false;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(GameManager)).Length >= 2) 
        {
            Destroy(gameObject);
            return;
        }

        GameLogger.Info("GameManager Awake");

        DontDestroyOnLoad(gameObject);

        _dataManager = new DataManager();
        AddManager(_dataManager);

        _sceneManager = new SceneManager();
        AddManager(_sceneManager, true);

        _inputManager = new InputManager();
        AddManager(_inputManager, true);

        APP.SceneManager = _sceneManager; 
        APP.UI = new UIManager();
        APP.InputManager = _inputManager;

        foreach (IManager manager in _managers)
        {
            manager.Init();
        }

        APP.InputManager.AddInputAction(EInputAction.PAUSE, StopManagers);
        APP.InputManager.AddInputAction(EInputAction.PLAY, PlayManagers);
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
        APP.UI.StartManager();
    }

    void Update()
    {
        for(int i=0; i<_managerUpdatables.Count; i++)
            _managerUpdatables[i].UpdateManager();
        
    }

    private void StopManagers()
    {
        _inputManager.SetStop(true);
    }
    private void PlayManagers()
    {
        _inputManager.SetStop(false);
    }

    private void AddManager<T>(T manager, bool isUpdatable = false) where T :IManager
    {
        _managers.Add(manager);
        if (isUpdatable)
            _managerUpdatables.Add((IManagerUpdatable)manager);
    }

}
