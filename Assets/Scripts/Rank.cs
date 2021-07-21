using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class RankInfo
{
    public string name;
    public string phoneNumber;
    public float playTime;
}

public class Rank : MonoBehaviour
{
    public static List<RankInfo> rankInfo = new List<RankInfo>();

    [SerializeField]
    private TextAsset rankJson;

    private void Awake()
    {
        Load();
    }

    public void Sort()
    {
        rankInfo.Sort((a, b) => a.playTime.CompareTo(b.playTime));
    }

    public void Save()
    {
        Sort();

        string toJson = JsonConvert.SerializeObject(rankInfo);

        PlayerPrefs.SetString("RankData", toJson);
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("RankData"))
        {
            PlayerPrefs.SetString("RankData", rankJson.text);
        }

        rankInfo = JsonConvert.DeserializeObject<List<RankInfo>>(PlayerPrefs.GetString("RankData"));
    }
}