
public class LobbyScene : SceneBase
{

    public Rule_Lobby Rule { get { if (_rule == null) { LOG.E("Rule Is Null"); } return _rule; } }
    public UI_LobbyScene UI { get { if (_ui == null) { LOG.E("UI Is Null"); } return _ui; } }

    private Rule_Lobby _rule = null;
    public UI_LobbyScene _ui = null;

    public LobbyScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override bool Enter()
    {
        _ui = APP.UI.ShowSceneUI<UI_LobbyScene>("UI_LobbyScene");
        if (!Rule_Lobby.Create(out _rule))
            return false;

        return true;
    }

    protected override void Exit()
    {
        Rule_Lobby.Destroy(ref _rule);
    }

    protected override void Start()
    {
        _rule.StartFirst();
    }

    protected override void Update()
    {
        _rule.Update();
    }
}
