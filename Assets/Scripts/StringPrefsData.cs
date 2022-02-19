using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringPrefsData
{
    public string Key;
    public string Value;

    public StringPrefsData(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
