using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public static Dictionary<string, string[]> recipes;

    public static string[] ingredients;

    [SerializeField]
    private TextAsset recipeJson;

    private void Awake()
    {
        recipes = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(recipeJson.text);
    }
}