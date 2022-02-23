using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RewardView : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private PlayerRewardDataHandler _dataSaver;
        [Header("Daily time settings")]
        [SerializeField]
        public int DailyTimeCooldown = 5;
        [SerializeField]
        public int DailyTimeDeadline = 10;
        [SerializeField]
        public int WeeklyTimeCooldown = 10;
        [SerializeField]
        public int WeeklyTimeDeadline = 20;
        [Space]
        [Header("RewardSettings")]
        public List<Reward> DailyRewards;
        public List<Reward> WeeklyRewards;
        [Header("UI")]
        [SerializeField]
        public Slider DailyRewardTimer;
        [SerializeField]
        public Slider WeeklyRewardTimer;
        [SerializeField]
        public Transform DailySlotsParent;
        [SerializeField]
        public Transform WeeklySlotsParent;
        [SerializeField]
        public SlotRewardView SlotPrefab;
        [SerializeField]
        public Button ResetButton;
        [SerializeField]
        public Button GetDailyRewardButton;
        [SerializeField]
        public Button GetWeeklyRewardButton;
        #endregion

        public int CurrentActiveSlotDaily
        {
            get => _dataSaver.ActiveSlotKeyDaily;
            set => _dataSaver.ActiveSlotKeyDaily = value;
        }

        public int CurrentActiveSlotWeekly
        {
            get => _dataSaver.ActiveSlotKeyWeekly;
            set => _dataSaver.ActiveSlotKeyWeekly = value;
        }

        public DateTime? LastRewardTime
        {
            get => _dataSaver.LastTimeKeyDaily;
            set => _dataSaver.LastTimeKeyDaily = value;
        }

        public DateTime? LastRewardTimeWeekly
        {
            get => _dataSaver.LastTimeKeyWeekly;
            set => _dataSaver.LastTimeKeyWeekly = value;
        }


        private void OnDestroy()
        {
            GetDailyRewardButton.onClick.RemoveAllListeners();
            GetWeeklyRewardButton.onClick.RemoveAllListeners();
            ResetButton.onClick.RemoveAllListeners();
        }

    }
}