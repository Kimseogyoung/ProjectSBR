using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class APP
{
    static public Config.Game GameConf { get; set; }
    static public Config.Debug DebugConf { get; set; }
    
    static public IGameLogger GameLogger { get; set; }

}


