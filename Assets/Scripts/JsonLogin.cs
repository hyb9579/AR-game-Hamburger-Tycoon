using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using System.IO;
using UnityEngine.UI;

public class JsonLogin : MonoBehaviour
{
    [SerializeField]
    public PlayerData[] playerDatas;

    private void Start()
    {
        Save();
        Load();
    }
   
    void Save()
    {
        string toJson = JsonHelper.ToJson<PlayerData>(playerDatas, prettyPrint: true);
        PlayerPrefs.SetString("PLAYER_DATA", toJson);
    }
    
    void Load()
    {
        string jsonData = PlayerPrefs.GetString("PLAYER_DATA");
        PlayerData[] playerDatas = JsonHelper.FromJson<PlayerData>(jsonData);
        
        for (int i = 0; i < playerDatas.Length; i++)
        {
            Debug.Log(i + " : " + playerDatas[i].NickName + ", " + playerDatas[i].PhoneNum);
        }
    }

}

