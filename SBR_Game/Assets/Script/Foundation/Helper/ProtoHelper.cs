using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class ProtoHelper 
{
    private static Dictionary<Type, Dictionary<object,object>> _protoDict = new Dictionary<Type, Dictionary<object, object>>(); //Proto, Key
    private static ProtoReader _reader;
    private static FileReader _fileReader = new FileReader();
    static public void Start()
    {
        _reader = new ProtoReader();
    }

    static public IEnumerable GetEnumerable<TProto>() where TProto : ProtoItem, new()
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out Dictionary<object, object> dict))
        {
            Bind<TProto>();
            GameLogger.Info($"{typeof(TProto).Name} is not exist. so Bind");
            dict = _protoDict[typeof(TProto)];
            //return default(TProto);
        }

        return dict.Values;
    }

    static public int GetCount<TProto>() where TProto : ProtoItem, new()
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out Dictionary<object, object> dict))
        {
            Bind<TProto>();
            GameLogger.Info($"{typeof(TProto).Name} is not exist. so Bind");
            dict = _protoDict[typeof(TProto)];
            //return default(TProto);
        }

        return dict.Count();
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

    public static List<TProto> GetAll<TProto>() where TProto : ProtoItem, new()
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out Dictionary<object, object> dict))
        {
            Bind<TProto>();
            GameLogger.Info($"{typeof(TProto).Name} is not exist. so Bind");
            dict = _protoDict[typeof(TProto)];
        }
        return dict.Values.Cast<TProto>().ToList();
    }

    static public void Bind<TProto>(string className = "", IComparer<TProto> comparer = null) where TProto : ProtoItem, new()
    {
        if(className == "") className= typeof(TProto).Name.Replace("Proto","");
        Type protoClassType = typeof(TProto);

        if (_protoDict.ContainsKey(typeof(TProto)))
        {
            GameLogger.Info($"{typeof(TProto).Name} is existed");
            return;
        }

        List<TProto> list = _reader.LoadCsv<TProto>(out Type pkType, out string pkName,
            _fileReader.ReadTextAsset(System.IO.Path.Join(/*Application.dataPath,*/ "Data/Proto/Csv", $"{className}")));

        _protoDict.Add(protoClassType, new Dictionary<object, object>());

        if(comparer != null)
            list.Sort(comparer);

        for (int i=0; i<list.Count; i++)
        {
            list[i].Idx = i;
            PropertyInfo property = typeof(TProto).GetProperty(pkName);
            _protoDict[protoClassType].Add(property.GetValue(list[i]), list[i]);
        }

    }
    
}
