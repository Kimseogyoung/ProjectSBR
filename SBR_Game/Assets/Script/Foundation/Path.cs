using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
    static public string ConfigDir { get; private set; } = "/Data/Config";
    static public string GameConfig { get; private set; } = "/Game.yaml";
    static public string DebugConfig { get; private set; } = "/Debug.yaml";
    static public string StageButton { get; private set; } = "UI/Scene/Lobby/StageButton";
    static public string StageStar { get; private set; } = "UI/Scene/Lobby/StageStars";



    //Image
    static public string CasualUI { get; private set; } = "Image/UI/GUI";
    static public string FilledStar { get; private set; } = "GUI_24";
    static public string EmptyStar { get; private set; } = "GUI_25";
}
