using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System;
using Unity.VisualScripting.FullSerializer;

public class YamlParser 
{ 
    public T GetConfig<T>(string data)  where T : Config.ConfigBase
    {
        var deserializer = new DeserializerBuilder()
           .Build();

        //yml contains a string containing your YAML
        T config = deserializer.Deserialize<T> (data);
        return config;
    }

}
