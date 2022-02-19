using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardSaveModel
{
    public List<IntPrefsData> IntPrefsData;
    public List<StringPrefsData> StringPrefsData;
    public RewardSaveModel()
    {
        IntPrefsData = new List<IntPrefsData>(PrefsKeys.IntKeys.Count);

        for (int i = 0; i < PrefsKeys.IntKeys.Count; i++)
        {
            if (PlayerPrefs.HasKey(PrefsKeys.IntKeys[i]))
            {
                var key = PrefsKeys.IntKeys[i];
                IntPrefsData.Add(new IntPrefsData(key, PlayerPrefs.GetInt(key)));
            }
        }

        StringPrefsData = new List<StringPrefsData>(PrefsKeys.StringKeys.Count);

        for (int i = 0; i < PrefsKeys.StringKeys.Count; i++)
        {
            if (PlayerPrefs.HasKey(PrefsKeys.StringKeys[i]))
            {
                var key = PrefsKeys.StringKeys[i];
                StringPrefsData.Add(new StringPrefsData(key, PlayerPrefs.GetString(key)));
            }
        }
    }

    public void InitSaveData()
    {
        for (int i = 0; i < IntPrefsData.Count; i++)
        {
            PlayerPrefs.SetInt(IntPrefsData[i].Key, IntPrefsData[i].Value);
        }

        for (int i = 0; i < StringPrefsData.Count; i++)
        {
            PlayerPrefs.SetString(StringPrefsData[i].Key, StringPrefsData[i].Value);
        }
    }
}
