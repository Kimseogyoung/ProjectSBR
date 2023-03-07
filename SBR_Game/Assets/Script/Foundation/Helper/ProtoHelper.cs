using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ProtoHelper 
{
    static private Dictionary<Type, Dictionary<object,object>> _protoDict = new Dictionary<Type, Dictionary<object, object>>(); //Proto, Key
    static private ProtoReader _reader;

    static public void Start()
    {
        _reader = new ProtoReader();
    }
    
    static public TProto Get<TProto, TKey>(TKey key) where TProto : ProtoItem, new()
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out Dictionary<object, object> dict))
        {
            Bind<TProto>();
            GameLogger.Info($"{typeof(TProto).Name} is not exist. so Bind");
            dict = _protoDict[typeof(TProto)];
            //return default(TProto);
        }

        if (!dict.TryGetValue(key, out object value))
        {
            GameLogger.Error($"{typeof(TProto).Name} proto key Not found : {key}");
            return default(TProto);
        }

        return (TProto)value;
    }

    static public TProto GetUsingIndex<TProto>(int idx) where TProto : ProtoItem, new()
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out Dictionary<object, object> dict))
        {
            Bind<TProto>();
            GameLogger.Info($"{typeof(TProto).Name} is not exist. so Bind");
            dict = _protoDict[typeof(TProto)];
        }

        foreach(ProtoItem value in dict.Values)
        {
            if (value.Idx == idx) return (TProto)value;
        }

        return default(TProto);
    }

    static private void Bind<TProto>(string className = "") where TProto : ProtoItem, new()
    {
        if(className == "") className= typeof(TProto).Name.Replace("Proto","");
        Type protoClassType = typeof(TProto);
        List<TProto> list = _reader.LoadCsv<TProto>(out Type pkType, out string pkName,
            _reader.ReadCsv(System.IO.Path.Join(Application.dataPath, "Data/Proto/Csv", $"{className}.csv")));

        _protoDict.Add(protoClassType, new Dictionary<object, object>());
        for (int i=0; i<list.Count; i++)
        {
            list[i].Idx = i;
            PropertyInfo property = typeof(TProto).GetProperty(pkName);
            _protoDict[protoClassType].Add(property.GetValue(list[i]), list[i]);
        }

    }
    
}
