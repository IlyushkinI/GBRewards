using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardController
{
    private readonly DailyRewardView _rewardView;
    private List<SlotRewardView> _slots;

    private bool _rewardReceived = false;
    private const string _dayTypeCount = "Day";
    private const string _weekTypeCount = "Week";
    public DailyRewardController(DailyRewardView rewardView)
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
        _rewardReceived = false;
        if (_rewardView.LastDailyRewardTime.HasValue)
        {
            var timeSpan = DateTime.UtcNow - _rewardView.LastDailyRewardTime.Value;
            if (timeSpan.Seconds > _rewardView.DayTimeDeadline)
            {
                _rewardView.LastDailyRewardTime = null;
                _rewardView.CurrentActiveSlot = 0;
            }
            else if(timeSpan.Seconds < _rewardView.DayTimeCooldown)
            {
                _rewardReceived = true;
            }
        }
    }

    private void RefreshUi()
    {
        _rewardView.GetRewardButton.interactable = !_rewardReceived;

        for (var i = 0; i < _rewardView.DailyRewards.Count; i++)
        {
            _slots[i].SetData(i <= _rewardView.CurrentActiveSlot);
        }

        DateTime nextDailyBonusTime = !_rewardView.LastDailyRewardTime.HasValue ? DateTime.MinValue
                : _rewardView.LastDailyRewardTime.Value.AddSeconds(_rewardView.DayTimeCooldown);

        var dayDelta = nextDailyBonusTime - DateTime.UtcNow;
        if (dayDelta.TotalSeconds < 0)
            dayDelta = new TimeSpan(0);

        _rewardView.DailyRewardTimer.text = dayDelta.ToString();
        _rewardView.DailyRewardTimerImage.fillAmount = 
                    (_rewardView.DayTimeCooldown - (float)dayDelta.TotalSeconds) / _rewardView.DayTimeCooldown;

        //DateTime nextWeeklyBonusTime = !_rewardView.LastWeeklyRewardTime.HasValue ? DateTime.MinValue
        //        : _rewardView.LastWeeklyRewardTime.Value.AddSeconds(_rewardView.WeekTimeCooldown);

        //var weekDelta = nextWeeklyBonusTime - DateTime.UtcNow;
        //if (weekDelta.TotalSeconds < 0)
        //    weekDelta = new TimeSpan(0);

        //_rewardView.WeeklyRewardTimer.text = weekDelta.ToString();
        //_rewardView.WeeklyRewardTimerImage.fillAmount = 
        //            (_rewardView.WeekTimeCooldown - (float)weekDelta.TotalSeconds) / _rewardView.WeekTimeCooldown;
    }

    private void InitSlots()
    {
        _slots = new List<SlotRewardView>();
        for (int i = 0; i < _rewardView.DailyRewards.Count; i++)
        {
            var reward = _rewardView.DailyRewards[i];
            var slotInstance = GameObject.Instantiate(_rewardView.SlotPrefab, _rewardView.DailySlotsParent, false);
            slotInstance.SetData(reward, _dayTypeCount, i + 1, false);
            _slots.Add(slotInstance);
        }
        for (int i = 0; i < _rewardView.WeeklyRewards.Count; i++)
        {
            var reward = _rewardView.WeeklyRewards[i];
            var slotInstance = GameObject.Instantiate(_rewardView.SlotPrefab, _rewardView.WeeklySlotsParent, false);
            slotInstance.SetData(reward, _weekTypeCount, i + 1, false);
            _slots.Add(slotInstance);
        }
    }

    private void SubscribeButtons()
    {
        _rewardView.GetRewardButton.onClick.AddListener(ClaimReward);
        _rewardView.ResetButton.onClick.AddListener(ResetReward);
        _rewardView.ShowDailyRewardsButton.onClick.AddListener(() => 
                    SetRewardWindow(_rewardView.DailySlotsParent, _rewardView.ShowDailyRewardsButton));
        _rewardView.ShowWeeklyRewardsButton.onClick.AddListener(() => 
                    SetRewardWindow(_rewardView.WeeklySlotsParent, _rewardView.ShowWeeklyRewardsButton));
    }

    private void SetRewardWindow(Transform grid, Button button)
    {
        if (button.image.color == Color.green)
            return;

        _rewardView.ResetRewardsShowCondition();
        button.image.color = Color.green;
        grid.gameObject.SetActive(true);
    }

    private void ResetReward()
    {
        _rewardView.LastDailyRewardTime = null;
        _rewardView.CurrentActiveSlot = 0;
    }

    private void ClaimReward()
    {
        if (_rewardReceived)
            return;
        var reward = _rewardView.DailyRewards[_rewardView.CurrentActiveSlot];
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

        _rewardView.LastDailyRewardTime = DateTime.UtcNow;
        _rewardView.CurrentActiveSlot = (_rewardView.CurrentActiveSlot + 1) % _rewardView.DailyRewards.Count;
        RefreshRewardState();
    }
}
