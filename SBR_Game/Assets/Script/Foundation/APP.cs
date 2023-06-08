public class APP
{
    static public Config.Game GameConf { get; set; }
    static public Config.Debug DebugConf { get; set; }

    static public StageProto CurrentStage { get; set; }

    //Manager
    static public GameManager GameManager { get; set; }
    static public ICharacters InGame { get; set; }
    static public IBullet Bullet { get; set; }
    static public InputManager InputManager { get; set; }
    static public SceneManager SceneManager { get; set; }

    static public UIManager UI { get; set; }
}


