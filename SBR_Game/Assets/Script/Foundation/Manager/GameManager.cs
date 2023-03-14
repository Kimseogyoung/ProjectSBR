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

    public T AddUpdatablePublicManager<T>(T manager) where T : IManagerUpdatable
    {
        _managerUpdatables.Add(manager);
        return manager;
    }

    public void RemoveUpdatablePublicManager<T>(T manager) where T : IManagerUpdatable
    {
        if (!_managerUpdatables.Contains(manager)) return;
        _managerUpdatables.Remove(manager);
    }

    private void Awake()
    {
        
        if (FindObjectsOfType(typeof(GameManager)).Length >= 2) 
        {
            Destroy(gameObject);
            return;
        }
        
        APP.GameManager = this;

        ProtoHelper.Start();

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

        APP.InputManager.AddInputAction(EInputAction.PAUSE, () => { Pause(new PauseEvent(true)); });
        APP.InputManager.AddInputAction(EInputAction.PLAY, () => { Pause(new PauseEvent(false)); });

        EventQueue.AddEventListener<PauseEvent>(EEventActionType.PAUSE, Pause);
        EventQueue.AddEventListener<PauseEvent>(EEventActionType.PLAY, Pause);
    }

    void Start()
    {

        foreach (IManager manager in _managers)
        {
            manager.StartManager();
        }
        APP.UI.StartManager();
    }

    void Update()
    {
        if (_isStopped)
        {
            for (int i = 0; i < _managerUpdatables.Count; i++)
                _managerUpdatables[i].UpdatePausedManager();
            return;
        }

        for (int i=0; i<_managerUpdatables.Count; i++)
            _managerUpdatables[i].UpdateManager();
        
    }

    private void Pause(PauseEvent pause)
    {

        if (pause.IsPause == _isStopped) return;
        _isStopped = pause.IsPause;

        for (int i = 0; i < _managerUpdatables.Count; i++)
            _managerUpdatables[i].Pause(pause.IsPause);
    }

    private void AddManager<T>(T manager, bool isUpdatable = false) where T :IManager
    {
        _managers.Add(manager);
        if (isUpdatable)
            _managerUpdatables.Add((IManagerUpdatable)manager);
    }

}
