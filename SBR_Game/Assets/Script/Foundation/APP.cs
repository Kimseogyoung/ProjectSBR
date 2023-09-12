public class APP
{
    public static LocalPlayerPrefs LocalPlayerPrefs { get; set; }
    static public Config.Game GameConf { get; set; }
    static public Config.Debug DebugConf { get; set; }

    //Manager
    static public GAME GAME { get; set; }
    static public ICharacters InGame { get; set; }
    static public IBullet Bullet { get; set; }
    static public InputManager InputManager { get; set; }
    static public SceneManager SceneManager { get; set; }

    static public UIManager UI { get; set; }
}


