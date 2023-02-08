using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
    static public string ConfigDirPath { get; private set; } = "/Data/Config";
    static public string GameConfigPath { get; private set; } = "/Game.yaml";
    static public string DebugConfigPath { get; private set; } = "/Debug.yaml";
}
