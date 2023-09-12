using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InGameScene : SceneBase
{
    public Rule_InGame Rule { get { if (_rule == null) { LOG.E("Rule Is Null"); } return _rule; } }
    public UI_InGameScene UI { get { if (_ui == null) { LOG.E("UI Is Null"); } return _ui; } }

    private Rule_InGame _rule;
    private UI_InGameScene _ui;

    public StageProto StagePrt { get; private set; }

    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override bool Enter()
    {
        StagePrt = APP.GAME.Player.GetCurStagePrt();

        if (!Rule_InGame.Create(out _rule))
            return false;

        _ui = APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");
        return true;
    }

    protected override void Exit()
    {
        Rule_InGame.Destroy(ref _rule);
        
    }

    protected override void Start()
    {
        _rule.StartFirst();
    }

    protected override void Update()
    {
        _ui.Refresh();
        _rule.Update();
    }

    
}
