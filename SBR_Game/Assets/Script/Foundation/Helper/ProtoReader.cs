using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using System;
using System.ComponentModel;

public class ProtoReader
{
    private Dictionary<string, Type> _types = new Dictionary<string, Type>();

    public ProtoReader()
    {
        _types.Add("int",GetTypeFromString("int"));
        _types.Add("float",GetTypeFromString("float"));
        _types.Add("str",GetTypeFromString("str"));
        _types.Add("bool",GetTypeFromString("bool"));
        _types.Add("double",GetTypeFromString("double"));

    }

    public List<T> LoadCsv<T>(out Type pkType,out string pkName, string text) where T : class, new()
    {
        string[] lines = text.Split("\n");


        List<string> names = lines[0].Split(",").ToList<string>();
        List<string> types = lines[1].Split(",").ToList<string>();

        

        List<List<string>> columns = new List<List<string>>();
        for (int i = 2; i < lines.Length; i++)
        {
            List<string> value = lines[i].Split(",").ToList<string>();
            if (value.Count < names.Count) continue;
            columns.Add(value);

        }

        pkName = names[0];
        pkType = GetTypeFromString(types[0].Split(":pk")[0]);

        List<T> results = new List<T>();
        for (int i = 0; i < columns.Count; i++)
        {
            T obj = new T();
            for (int j = 0; j < names.Count; j++)
            {
                string propertyName = names[j];
                if (propertyName.StartsWith("#")) continue;

                string typeString = types[j];
                GameLogger.Strong("{0} {1}", i, j);
                string value = columns[i][j];


                if (value == string.Empty) continue;

                PropertyInfo property = typeof(T).GetProperty(propertyName);
                if(property == null)
                {
                    GameLogger.Error($"property Null {propertyName}");
                    return null;
                }
                
                if (typeString.EndsWith(":pk")){
                    typeString = typeString.Split(":pk")[0];
                }

                property.SetValue(obj, ConvertToType(columns[i][j], _types[typeString]));
            }
            results.Add(obj);
        }
        return results;
    }

    public string ReadCsv(string filePath)
    {
        string text = string.Empty;
        try
        {
            text = File.ReadAllText(Path.Join(Application.dataPath, "Data/Proto/Csv", filePath));
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message);
        }
        return text;
    }

    private Type GetTypeFromString(string typeString)
    {
        switch (typeString)
        {
            case "int":
                return typeof(int);
            case "double":
                return typeof(double);
            case "bool":
                return typeof(bool);
            case "str":
                return typeof(string);
            case "float":
                return typeof(string);
            default:
                throw new Exception($"Unsupported type: {typeString}");
        }
    }

    private object ConvertToType(string field, Type type)
    {
        try
        {
            return TypeDescriptor.GetConverter(type).ConvertFrom(field);
        }
        catch (Exception e)
        {
            throw new Exception($"Unsupported type: {type.Name} {field} {e}");
        }
    }
}
