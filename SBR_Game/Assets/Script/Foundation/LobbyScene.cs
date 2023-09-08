
public class LobbyScene : SceneBase
{

    public Rule_Lobby _rule = null;
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
