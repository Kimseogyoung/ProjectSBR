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
    public ProtoReader()
    {

    }

    public List<T> LoadCsv<T>(out Type pkType, out string pkName, string text) where T : class, new()
    {
        string[] lines = text.Split("\r\n");
        pkType = null;
        pkName = string.Empty;

        if (!LoadCsvField(out pkName, out List<string> names, out List<string> types, text))
        {
            GameLogger.Error("Load Csv Error");
            return null;
        }
        

        List<List<string>> columns = new List<List<string>>();
        for (int i = 2; i < lines.Length; i++)
        {
            List<string> value = lines[i].Split(",").ToList<string>();
            if (value.Count < names.Count) continue;
            columns.Add(value);

        }

        pkType = GetTypeFromString(types[0]);

        List<T> results = new List<T>();
        for (int i = 0; i < columns.Count; i++)
        {
            T obj = new T();
            for (int j = 0; j < names.Count; j++)
            {
                string propertyName = names[j];
                string typeString = types[j];

                string value = columns[i][j];


                if (value == string.Empty) continue;

                PropertyInfo property = typeof(T).GetProperty(propertyName);
                if(property == null)
                {
                    GameLogger.Error($"property Null {propertyName}");
                    return null;
                }

                property.SetValue(obj, ConvertToType(columns[i][j], GetTypeFromString(typeString)));
            }
            results.Add(obj);
        }
        return results;
    }

    public bool LoadCsvField(out string pkName, out List<string> fieldNames, out List<string> fieldTypes, string text)
    {
        string[] lines = text.Split("\r\n");
        pkName = string.Empty;
        fieldNames = new List<string>();
        fieldTypes = new List<string>();

        List<string> names = lines[0].Split(",").ToList<string>();
        List<string> types = lines[1].Split(",").ToList<string>();

        try
        {
            for (int i = 0; i < names.Count; i++)
            {
                
                if (names[i].StartsWith("#")) continue;
                fieldNames.Add(names[i]);

                if (types[i].EndsWith(":pk")) 
                { 
                    types[i] = types[i].Replace(":pk", "");
                    pkName = names[i];
                }

                
                fieldTypes.Add(types[i]);
            }
            return true;
        }
        catch(Exception e)
        {
            GameLogger.Error(e.Message);
            return false;
        }
    }

    public string ReadCsv(string filePath)
    {
        string text = string.Empty;
        try
        {
            text = File.ReadAllText(filePath);
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message);
        }
        return text;
    }

    private Type GetTypeFromString(string typeString)
    {
        GameLogger.Info($"{typeString}");
        switch (typeString)
        {
            case "int":
                return typeof(int);
            case "double":
                return typeof(double);
            case "bool":
                return typeof(bool);
            case "string":
                return typeof(string);
            case "float":
                return typeof(float);
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
            throw new Exception($"Unsupported type(ConvertToType): {type.Name} {field} {e}");
        }
    }
}
