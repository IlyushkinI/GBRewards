using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardController
{
    private readonly RewardView _rewardView;
    private List<SlotRewardView> _slots;

    private bool _rewardReceived = false;

    public DailyRewardController(RewardView rewardView)
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
        if (_rewardView.LastRewardDailyTime.HasValue)
        {
            var timeSpan = DateTime.UtcNow - _rewardView.LastRewardDailyTime.Value;
            if (timeSpan.Seconds > _rewardView.TimeDailyDeadline)
            {
                _rewardView.LastRewardDailyTime = null;
                _rewardView.CurrentActiveDailySlot = 0;
            }
            else if(timeSpan.Seconds < _rewardView.TimeDailyCooldown)
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
            _slots[i].SetData(_rewardView.DailyRewards[i], i+1, i <= _rewardView.CurrentActiveDailySlot);
        }

        DateTime nextDailyBonusTime =
            !_rewardView.LastRewardDailyTime.HasValue
                ? DateTime.MinValue
                : _rewardView.LastRewardDailyTime.Value.AddSeconds(_rewardView.TimeDailyCooldown);
        var delta = nextDailyBonusTime - DateTime.UtcNow;
        if (delta.TotalSeconds < 0)
            delta = new TimeSpan(0);

        _rewardView.RewardDailyTimer.value = (float)delta.Seconds/ (float)_rewardView.TimeDailyCooldown;
    }

    private void InitSlots()
    {
        _slots = new List<SlotRewardView>();
        for (int i = 0; i < _rewardView.DailyRewards.Count; i++)
        {
            var reward = _rewardView.DailyRewards[i];
            var slotInstance = GameObject.Instantiate(_rewardView.SlotPrefab, _rewardView.SlotsParent, false);
            slotInstance.SetData(reward, i+1, false);
            _slots.Add(slotInstance);
        }
    }

    private void SubscribeButtons()
    {
        _rewardView.GetRewardButton.onClick.AddListener(() => ClaimReward(RewardDateTime.Daily));
        _rewardView.ResetButton.onClick.AddListener(ResetReward);
    }

    private void ResetReward()
    {
        _rewardView.LastRewardDailyTime = null;
        _rewardView.CurrentActiveDailySlot = 0;
    }

    private void ClaimReward(RewardDateTime rewardDateTime)
    {
        if (_rewardReceived)
        {
            return;
        }
            
        var reward = _rewardView.DailyRewards[_rewardView.CurrentActiveDailySlot];
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

        _rewardView.LastRewardDailyTime = DateTime.UtcNow;
        _rewardView.CurrentActiveDailySlot = (_rewardView.CurrentActiveDailySlot + 1) % _rewardView.DailyRewards.Count;
        RefreshRewardState();
    }

   
}
