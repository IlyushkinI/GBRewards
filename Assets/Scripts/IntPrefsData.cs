using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntPrefsData
{
    public string Key;
    public int Value;

    public IntPrefsData(string key, int value)
    {
        Key = key;
        Value = value;
    }
}
