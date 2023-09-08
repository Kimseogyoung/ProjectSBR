using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalPlayerPrefs : ClassBase
{
    public string PlayerJson { get; set; }
    protected override bool OnCreate()
    {
        LoadAll();
        return true;
    }

    protected override void OnDestroy()
    {
       
    }

    public void LoadAll()
    {
        
        FieldInfo[] fields = typeof(LocalPlayerPrefs).GetFields();

        foreach (var field in fields)
        {
            string value = PlayerPrefs.GetString(field.Name);
            if (string.IsNullOrEmpty(value))
                continue;

            field.SetValue(this, value);
            LOG.I($"Load LocalPlayerPrefs Key({field.Name}) Value({field.GetValue(this)})");
        } 
    }

    public void SavePlayerJson(Player player)
    {
        string json = JsonConvert.SerializeObject(player);
        PlayerPrefs.SetString(nameof(PlayerJson), json);
    }
}
