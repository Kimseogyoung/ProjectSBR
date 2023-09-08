using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SBRTool: MonoBehaviour
{

    [MenuItem("SBR Tools/Clear PlayerPrefs")]
    static void OpenIntroScene()
    {
        PlayerPrefs.DeleteAll();
    }

}

