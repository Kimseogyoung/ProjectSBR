using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Security.Principal;

public class SBRTool: MonoBehaviour
{

    [MenuItem("SBR Tools/Clear PlayerPrefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("SBR Tools/Open PlayerPrefs")]
    static void OpenPlayerPrefs()
    {
        File.Open($"C:\\{WindowsIdentity.GetCurrent().Name}\\Software\\Unity\\UnityEditor\\{Application.companyName}\\{Application.productName}", FileMode.OpenOrCreate);
    }

}

