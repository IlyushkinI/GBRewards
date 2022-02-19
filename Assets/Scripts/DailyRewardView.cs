using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardView : MonoBehaviour
{
    private const string LastDayTimeKey = "LastDailyRewardTime";
    private const string LastWeekTimeKey = "LastWeeklyRewardTime";
    private const string ActiveSlotKey = "ActiveSlot";

    #region Fields
    [Header("Time settings")]
    [SerializeField] public int DayTimeCooldown = 86400;
    [SerializeField] public int WeekTimeCooldown = 604800;
    [SerializeField] public int DayTimeDeadline = 172800;
    [SerializeField] public int WeekTimeDeadline = 345600;
    [Space]

    [Header("RewardSettings")]
    public List<Reward> DailyRewards;
    public List<Reward> WeeklyRewards;

    [Header("UI")]
    [SerializeField] public TMP_Text DailyRewardTimer;
    [SerializeField] public TMP_Text WeeklyRewardTimer;
    [SerializeField] public Transform DailySlotsParent;
    [SerializeField] public Transform WeeklySlotsParent;
    [SerializeField] public SlotRewardView SlotPrefab;
    [SerializeField] public Button ResetButton;
    [SerializeField] public Button GetRewardButton;
    [SerializeField] public Button ShowDailyRewardsButton;
    [SerializeField] public Button ShowWeeklyRewardsButton;
    [SerializeField] public Image DailyRewardTimerImage;
    [SerializeField] public Image WeeklyRewardTimerImage;
    #endregion

    public int CurrentActiveSlot
    {
        get => PlayerPrefs.GetInt(ActiveSlotKey);
        set => PlayerPrefs.SetInt(ActiveSlotKey, value);
    }

    public DateTime? LastDailyRewardTime
    {
        get
        {
            var data = PlayerPrefs.GetString(LastDayTimeKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(LastDayTimeKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(LastDayTimeKey);
        }
    }

    public DateTime? LastWeeklyRewardTime
    {
        get
        {
            var data = PlayerPrefs.GetString(LastWeekTimeKey);
            if (string.IsNullOrEmpty(data))
                return null;
            return DateTime.Parse(data);
        }
        set
        {
            if (value != null)
                PlayerPrefs.SetString(LastWeekTimeKey, value.ToString());
            else
                PlayerPrefs.DeleteKey(LastWeekTimeKey);
        }
    }


    public void ResetRewardsShowCondition()
    {
        DailySlotsParent.gameObject.SetActive(false);
        WeeklySlotsParent.gameObject.SetActive(false);
        ShowDailyRewardsButton.image.color = Color.white;
        ShowWeeklyRewardsButton.image.color = Color.white;
    }

    private void OnDestroy()
    {
        GetRewardButton.onClick.RemoveAllListeners();
        ResetButton.onClick.RemoveAllListeners();
        ShowDailyRewardsButton.onClick.RemoveAllListeners();
        ShowWeeklyRewardsButton.onClick.RemoveAllListeners();
    }
}
