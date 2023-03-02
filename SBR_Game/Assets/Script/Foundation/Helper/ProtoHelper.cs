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
        Bind<StageProto>();
    }
    
    static public TProto Get<TProto, TKey>(TKey key)
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out Dictionary<object, object> dict))
        {
            GameLogger.Error($"{typeof(TProto).Name} is not exist");
            return default(TProto);
        }

        if (!dict.TryGetValue(key, out object value))
        {
            GameLogger.Error($"{typeof(TProto).Name} proto key Not found : {key}");
            return default(TProto);
        }

        return (TProto)value;
    }

    static private void Bind<TProto>(string className = "") where TProto : class, new()
    {
        if(className == "") className= typeof(TProto).Name.Replace("Proto","");
        Type protoClassType = typeof(TProto);
        List<TProto> list = _reader.LoadCsv<TProto>(out Type pkType, out string pkName,
            _reader.ReadCsv($"{className}.csv"));

        _protoDict.Add(protoClassType, new Dictionary<object, object>());
        for (int i=0; i<list.Count; i++)
        {
            PropertyInfo property = typeof(TProto).GetProperty(pkName);
            _protoDict[protoClassType].Add(property.GetValue(list[i]), list[i]);
        }

    }
    
}