using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SBRMenu: MonoBehaviour
{

    [MenuItem("SBR/Open Intro Scene")]
    static void OpenIntroScene()
    {
        EditorSceneManager.OpenScene("Assets/Scene/IntroScene.unity");
    }

    [MenuItem("SBR/Open Lobby Scene")]
    static void OpenLobbyScene()
    {
        EditorSceneManager.OpenScene("Assets/Scene/LobbyScene.unity");
    }

    [MenuItem("SBR/Open Game Scene")]
    static void OpenGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scene/InGameScene.unity");
    }

}

