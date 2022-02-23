using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class RewardController
    {
        private readonly RewardView _rewardView;
        private List<SlotRewardView> _dailySlots;
        private List<SlotRewardView> _weeklySlots;

        private bool _dailyRewardReceived = false;
        private bool _weeklyRewardReceived = false;

        public RewardController(RewardView rewardView)
        {
            _rewardView = rewardView;
            InitSlots();
            RefreshUi();
            _rewardView.StartCoroutine(UpdateCoroutine());
            SubscribeButtons();
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                Update();
                yield return new WaitForSeconds(1);
            }
        }

        private void Update()
        {
            RefreshRewardState();
            RefreshUi();
        }

        private void RefreshRewardState()
        {
            _dailyRewardReceived = false;

            if (_rewardView.LastRewardTime.HasValue)
            {
                var timeSpan = DateTime.UtcNow - _rewardView.LastRewardTime.Value;
                if (timeSpan.Seconds > _rewardView.DailyTimeDeadline)
                {
                    _rewardView.LastRewardTime = null;
                    _rewardView.CurrentActiveSlotDaily = 0;
                }
                else if (timeSpan.Seconds < _rewardView.DailyTimeCooldown)
                {
                    _dailyRewardReceived = true;
                }
            }

            _weeklyRewardReceived = false;
            if (_rewardView.LastRewardTimeWeekly.HasValue)
            {
                var timeSpan = DateTime.UtcNow - _rewardView.LastRewardTimeWeekly.Value;
                if (timeSpan.Seconds > _rewardView.WeeklyTimeDeadline)
                {
                    _rewardView.LastRewardTimeWeekly = null;
                    _rewardView.CurrentActiveSlotWeekly = 0;
                }
                else if (timeSpan.Seconds < _rewardView.WeeklyTimeCooldown)
                {
                    _weeklyRewardReceived = true;
                }
            }
        }

        private void RefreshUi()
        {
            _rewardView.GetDailyRewardButton.interactable = !_dailyRewardReceived;

            for (var i = 0; i < _rewardView.DailyRewards.Count; i++)
            {
                _dailySlots[i].SetData(_rewardView.DailyRewards[i], i + 1, i <= _rewardView.CurrentActiveSlotDaily);
            }

            DateTime nextDailyBonusTime =
                !_rewardView.LastRewardTime.HasValue
                    ? DateTime.MinValue
                    : _rewardView.LastRewardTime.Value.AddSeconds(_rewardView.DailyTimeCooldown);
            var delta = nextDailyBonusTime - DateTime.UtcNow;
            if (delta.TotalSeconds < 0)
                delta = new TimeSpan(0);

            _rewardView.DailyRewardTimer.value = 1 - Convert.ToSingle(delta.Seconds) / Convert.ToSingle(_rewardView.DailyTimeCooldown);

            for (var i = 0; i < _rewardView.DailyRewards.Count; i++)
            {
                _dailySlots[i].SetData(_rewardView.DailyRewards[i], i + 1, i <= _rewardView.CurrentActiveSlotDaily);
            }

            _rewardView.GetWeeklyRewardButton.interactable = !_weeklyRewardReceived;

            DateTime nextWeeklyBonusTime =
                !_rewardView.LastRewardTimeWeekly.HasValue
                    ? DateTime.MinValue
                    : _rewardView.LastRewardTimeWeekly.Value.AddSeconds(_rewardView.WeeklyTimeCooldown);
            var deltaWeekly = nextWeeklyBonusTime - DateTime.UtcNow;
            if (deltaWeekly.TotalSeconds < 0)
                deltaWeekly = new TimeSpan(0);

            _rewardView.WeeklyRewardTimer.value = 1 - Convert.ToSingle(deltaWeekly.Seconds) / Convert.ToSingle(_rewardView.WeeklyTimeCooldown);

            for (var i = 0; i < _rewardView.WeeklyRewards.Count; i++)
            {
                _weeklySlots[i].SetData(_rewardView.WeeklyRewards[i], i + 1, i <= _rewardView.CurrentActiveSlotWeekly);
            }
        }

        private void InitSlots()
        {
            _dailySlots = new List<SlotRewardView>();
            for (int i = 0; i < _rewardView.DailyRewards.Count; i++)
            {
                var reward = _rewardView.DailyRewards[i];
                var slotInstance = UnityEngine.Object.Instantiate(_rewardView.SlotPrefab, _rewardView.DailySlotsParent, false);
                slotInstance.SetData(reward, i + 1, false);
                _dailySlots.Add(slotInstance);
            }
            _weeklySlots = new List<SlotRewardView>();
            for (int i2 = 0; i2 < _rewardView.WeeklyRewards.Count; i2++)
            {
                var reward = _rewardView.WeeklyRewards[i2];
                var slotInstanceWeekly = UnityEngine.Object.Instantiate(_rewardView.SlotPrefab, _rewardView.WeeklySlotsParent, false);
                slotInstanceWeekly.SetData(reward, i2 + 1, false);
                _weeklySlots.Add(slotInstanceWeekly);
            }
        }

        private void SubscribeButtons()
        {
            _rewardView.GetDailyRewardButton.onClick.AddListener(ClaimReward);
            _rewardView.GetWeeklyRewardButton.onClick.AddListener(ClaimWeeklyReward);
            _rewardView.ResetButton.onClick.AddListener(ResetReward);
        }

        private void ResetReward()
        {
            _rewardView.LastRewardTime = null;
            _rewardView.LastRewardTimeWeekly = null;
            _rewardView.CurrentActiveSlotDaily = 0;
            _rewardView.CurrentActiveSlotWeekly = 0;
        }

        private void ClaimReward()
        {
            if (_dailyRewardReceived)
                return;
            var reward = _rewardView.DailyRewards[_rewardView.CurrentActiveSlotDaily];
            switch (reward.Type)
            {
                case RewardType.None:
                    break;
                case RewardType.Wood:
                    CurrencyWindow.Instance.AddWood(reward.Count);
                    break;
                case RewardType.Diamond:
                    CurrencyWindow.Instance.AddDiamond(reward.Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _rewardView.LastRewardTime = DateTime.UtcNow;
            _rewardView.CurrentActiveSlotDaily = (_rewardView.CurrentActiveSlotDaily + 1) % _rewardView.DailyRewards.Count;
            RefreshRewardState();
        }

        private void ClaimWeeklyReward()
        {
            if (_weeklyRewardReceived)
                return;
            var reward = _rewardView.WeeklyRewards[_rewardView.CurrentActiveSlotWeekly];
            switch (reward.Type)
            {
                case RewardType.None:
                    break;
                case RewardType.Wood:
                    CurrencyWindow.Instance.AddWood(reward.Count);
                    break;
                case RewardType.Diamond:
                    CurrencyWindow.Instance.AddDiamond(reward.Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _rewardView.LastRewardTimeWeekly = DateTime.UtcNow;
            _rewardView.CurrentActiveSlotWeekly = (_rewardView.CurrentActiveSlotWeekly + 1) % _rewardView.WeeklyRewards.Count;
            RefreshRewardState();
        }
    }
}