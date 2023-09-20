using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using YamlDotNet.Core.Tokens;

public class ProtoHelper
{
    private class ProtoList
    {
        public List<object> List= new List<object>();
        public Dictionary<object, int> IdxDict= new Dictionary<object, int>();
    }

    private static Dictionary<Type, ProtoList> _protoDict = new Dictionary<Type, ProtoList>();
    private static ProtoReader _reader;
    private static FileReader _fileReader = new FileReader();

    static public void Start()
    {
        _reader = new ProtoReader();
    }

    static public IEnumerable GetEnumerable<TProto>() where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();
        return protoData.List;
    }

    static public int GetCount<TProto>() where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();
        return protoData.List.Count();
    }

    static public bool TryGet<TProto>(object key, out TProto prt) where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();
        prt = null;
        if (!protoData.IdxDict.TryGetValue(key, out int idx))
        {
            LOG.E($"{typeof(TProto).Name} proto key Not found : {key}");
            return false;
        }
        prt = (TProto)protoData.List[idx];
        return true;
    }

    static public TProto Get<TProto>(object key) where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();

        if (!protoData.IdxDict.TryGetValue(key, out int idx))
        {
            LOG.E($"{typeof(TProto).Name} proto key Not found : {key}");
            return default(TProto);
        }

        return (TProto)protoData.List[idx];
    }

    static public TProto GetNext<TProto>(TProto prt) where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();

        var idx = protoData.List.IndexOf(prt);
        if (protoData.List.Count <= idx + 1)
            return prt;

        return (TProto)protoData.List[idx + 1];
    }

    static public int GetIndexOf<TProto>(TProto prt) where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();

        var idx = protoData.List.IndexOf(prt);

        return idx;
    }

    static public TProto GetByIndex<TProto>(int idx) where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();
        return (TProto)protoData.List[idx];
    }

    static public TProto GetFirst<TProto>() where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();
        return (TProto)protoData.List[0];
    }

    public static List<TProto> GetAll<TProto>() where TProto : ProtoItem, new()
    {
        var protoData = GetPrtDict<TProto>();
        return protoData.List.Cast<TProto>().ToList();
    }

    static public void Bind<TProto>(string className = "", IComparer<TProto> comparer = null) where TProto : ProtoItem, new()
    {
        if(className == "") className= typeof(TProto).Name.Replace("Proto","");
        Type protoClassType = typeof(TProto);

        if (_protoDict.ContainsKey(typeof(TProto)))
        {
            LOG.I($"{typeof(TProto).Name} is existed");
            return;
        }

        List<TProto> list = _reader.LoadCsv<TProto>(out Type pkType, out string pkName,
            _fileReader.ReadTextAsset(System.IO.Path.Join(/*Application.dataPath,*/ "Data/Proto/Csv", $"{className}")));

        _protoDict.Add(protoClassType, new ProtoList());

        if(comparer != null)
            list.Sort(comparer);

        for (int i=0; i<list.Count; i++)
        {
            list[i].Idx = i;
            PropertyInfo property = typeof(TProto).GetProperty(pkName);
            _protoDict[protoClassType].List.Add(list[i]);
            _protoDict[protoClassType].IdxDict.Add(property.GetValue(list[i]), i);
        }

    }

    private static ProtoList GetPrtDict<TProto>() where TProto : ProtoItem, new ()
    {
        if (!_protoDict.TryGetValue(typeof(TProto), out ProtoList list))
        {
            Bind<TProto>();
            LOG.I($"{typeof(TProto).Name} is not exist. so Bind");
            list = _protoDict[typeof(TProto)];
        }
        return list;
    }
    
}
