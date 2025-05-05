using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static int SaveFileID = 0; // You can set this when loading a save file.

    private void Start()
    {
        SaveFileID = PlayerPrefs.GetInt("Save File ID", 0);
    }

    private static string Prefix(string key)
    {
        return "Save" + SaveFileID + key;
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(Prefix(key), value);
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(Prefix(key), defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(Prefix(key), value);
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(Prefix(key), defaultValue);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(Prefix(key), value);
    }

    public static string GetString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(Prefix(key), defaultValue);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(Prefix(key));
    }
}
