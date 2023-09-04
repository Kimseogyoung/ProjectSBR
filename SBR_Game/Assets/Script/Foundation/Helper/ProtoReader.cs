using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using System.ComponentModel;

public class ProtoReader
{
    public ProtoReader()
    {
        RegisterType();
    }

    public List<T> LoadCsv<T>(out Type pkType, out string pkName, string text) where T : class, new()
    {
        string[] lines = text.Split("\r\n");
        pkType = null;
        pkName = string.Empty;

        if (!LoadCsvField(out pkName, out List<string> names, out List<string> types, text))
        {
            GameLogger.E("Load Csv Error");
            return null;
        }
        

        List<List<string>> columns = new List<List<string>>();
        for (int i = 2; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#"))
                continue;
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
                    GameLogger.E($"property Null {propertyName}");
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
            GameLogger.E(e.Message);
            return false;
        }
    }

    private void RegisterType()
    {
        _typeMappingDict.Add("string", typeof(string));
        _typeMappingDict.Add("int", typeof(int));
        _typeMappingDict.Add("float", typeof(float));
        _typeMappingDict.Add("double", typeof(double));
        _typeMappingDict.Add("bool", typeof(bool));
        _typeMappingDict.Add($"enum:{nameof(ECharacterType)}", typeof(ECharacterType));
        _typeMappingDict.Add($"enum:{nameof(EAttack)}", typeof(EAttack));
        _typeMappingDict.Add($"enum:{nameof(EStat)}", typeof(EStat));
        _typeMappingDict.Add($"enum:{nameof(EHitShapeType)}", typeof(EHitShapeType));
        _typeMappingDict.Add($"enum:{nameof(EHitSKillType)}", typeof(EHitSKillType));
        _typeMappingDict.Add($"enum:{nameof(ECharacterTeamType)}", typeof(ECharacterTeamType));
        _typeMappingDict.Add($"enum:{nameof(ESkillType)}", typeof(ESkillType));
        _typeMappingDict.Add($"enum:{nameof(EHitTargetSelectType)}", typeof(EHitTargetSelectType));
        _typeMappingDict.Add($"enum:{nameof(EBuffType)}", typeof(EBuffType));
        _typeMappingDict.Add($"enum:{nameof(EItemType)}", typeof(EItemType));

    }

    private Type GetTypeFromString(string typeString)
    {
        if(!_typeMappingDict.TryGetValue(typeString, out var type))
        {
            throw new Exception($"Unsupported type: {typeString}");
        }
        return type;
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
    private Dictionary<string, Type> _typeMappingDict= new Dictionary<string, Type>();
}
