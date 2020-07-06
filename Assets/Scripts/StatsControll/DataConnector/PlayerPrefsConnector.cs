using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsConnector : MonoBehaviour, IStoreData
{
    public int LoadInt(string name) => PlayerPrefs.GetInt(name, 0);

    public string LoadString(string name) => PlayerPrefs.GetString(name, "default");

    public void SaveInt(int value, string name)
    {
        PlayerPrefs.SetInt(name, value);
        PlayerPrefs.Save();
    }

    public void SaveString(string value, string name)
    {
        PlayerPrefs.SetString(name, value);
        PlayerPrefs.Save();
    }
}
