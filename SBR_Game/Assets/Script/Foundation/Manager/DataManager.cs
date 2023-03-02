using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DataManager :IManager
{
    private YamlParser _yamlParser = new YamlParser();
    private FileReader _fileReader = new FileReader();

    public void Init()
    {
        APP.DebugConf = _yamlParser.GetConfig<Config.Debug>(_fileReader.ReadFile(AppPath.ConfigDir + AppPath.DebugConfig));
        APP.GameConf = _yamlParser.GetConfig<Config.Game>(_fileReader.ReadFile(AppPath.ConfigDir + AppPath.GameConfig));
    }


    public void StartManager()
    {

    }

    public void FinishManager()
    {

    }
}
