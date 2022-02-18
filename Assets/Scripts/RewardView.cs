using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    private const string LastTimeDailyKey = "LastDailyRewardTime";
    private const string ActiveDailySlotKey = "ActiveDailySlot";
    private const string LastTimeWeekKey = "LastWeekRewardTime";
    private const string ActiveWeekSlotKey = "ActiveWeekSlot";

    #region Fields
    [Header("Time settings")]
    [SerializeField]
    public int TimeDailyCooldown = 86400;
    [SerializeField]
    public int TimeDailyDeadline = 172800;
    [SerializeField]
    public int TimeWeekCooldown = 86400;
    [SerializeField]
    public int TimeWeekDeadline = 172800;
    [Space]
    [Header("RewardSettings")]
    public List<Reward> DailyRewards;
    public List<Reward> WeekRewards;
    [Header("UI")]
    [SerializeField]
    public TMP_Text RewardDailyTimer;
    [SerializeField]
    public TMP_Text RewardWeekTimer;
    [SerializeField]
    public Transform SlotsParent;
    [SerializeField]
    public SlotRewardView SlotPrefab;
    [SerializeField]
    public Button ResetButton;
    [SerializeField]
    public Button GetRewardButton;
    [SerializeField]
    public Button GetWeekRewardButton;
    #endregion

    public int CurrentActiveDailySlot
    {
        get => PlayerPrefs.GetInt(ActiveDailySlotKey);
        set => PlayerPrefs.SetInt(ActiveDailySlotKey, value);
    }

    public int CurrentActiveWeekSlot
    {
        get => PlayerPrefs.GetInt(ActiveWeekSlotKey);
        set => PlayerPrefs.SetInt(ActiveWeekSlotKey, value);
    }

    public DateTime? LastRewardDailyTime
    {
        get
        {
            var data = PlayerPrefs.GetString(LastTimeDailyKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(LastTimeDailyKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(LastTimeDailyKey);
        }
    }
    public DateTime? LastRewardWeekTime
    {
        get
        {
            var data = PlayerPrefs.GetString(LastTimeWeekKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(LastTimeWeekKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(LastTimeWeekKey);
        }
    }


    private void OnDestroy()
    {
        GetRewardButton.onClick.RemoveAllListeners();
        ResetButton.onClick.RemoveAllListeners();
        GetWeekRewardButton.onClick.RemoveAllListeners();
    }

}
