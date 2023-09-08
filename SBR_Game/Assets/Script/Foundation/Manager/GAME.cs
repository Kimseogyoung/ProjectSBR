using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAME: MonoBehaviour
{
    public enum EGameState
    {
        PREPARE,
        PLAY,
        STOP
    }

    public Player Player { get { if (_player == null) { LOG.E("Player Is Null"); } return _player; } }

    private List<IManagerUpdatable> _managerUpdatables = new List<IManagerUpdatable>();
    private List<IManager> _managers = new List<IManager>();
    private DataManager _dataManager;
    private SceneManager _sceneManager;
    private InputManager _inputManager;

    private EGameState _state = EGameState.PREPARE;
    private Player _player;

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
        if (FindObjectsOfType(typeof(GAME)).Length >= 2) 
        {
            Destroy(gameObject);
            return;
        }
        
        APP.GameManager = this;

        ProtoHelper.Start();

        LOG.I("GameManager Awake");

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
        LOG.I("StartManager");
        CreatePlayer(); // 이거 수정 필요
        APP.UI.StartManager();
        foreach (IManager manager in _managers)
        {
            manager.StartManager();
        }

        Init();
    }

    void FixedUpdate()
    {

        if (_state == EGameState.PREPARE)
            return;

        if(_state == EGameState.STOP)
        {
            for (int i = 0; i < _managerUpdatables.Count; i++)
                _managerUpdatables[i].UpdatePausedManager();
            return;
        }

        for (int i = 0; i < _managerUpdatables.Count; i++)
            _managerUpdatables[i].UpdateManager();

    }

    private async void Init()
    {
        bool result = await _sceneManager.StartFirstScene();

        if (!result)
        {
            LOG.E("Failed Init. GAME.Init()");
            return;
        }

        _state = EGameState.PLAY;
    }

    private void CreatePlayer()
    {
        _player = new Player();
        _player.Create();
    }

    private void Pause(PauseEvent pause)
    {

        if ((pause.IsPause && _state == EGameState.STOP) || (!pause.IsPause && _state == EGameState.PLAY)) 
            return;


        TimeHelper.Stop(pause.IsPause);
        _state = pause.IsPause? EGameState.STOP: EGameState.PLAY;

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
