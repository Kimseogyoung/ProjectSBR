using Newtonsoft.Json;
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
        foreach (var propertyInfo in this.GetType().GetProperties())
        {
            string value = PlayerPrefs.GetString(propertyInfo.Name);

            if (string.IsNullOrEmpty(value))
                continue;

            propertyInfo.SetValue(this, value);
            LOG.I($"Load LocalPlayerPrefs Key({propertyInfo.Name}) Value({propertyInfo.GetValue(this)})");
        } 
    }

    public void SavePlayerJson(Player player)
    {

        if (player == null)
            return;

        string json = JsonConvert.SerializeObject(player);
        PlayerPrefs.SetString(nameof(PlayerJson), json);
    }
}
