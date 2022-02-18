using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekRewardController
{
    private readonly RewardView _rewardView;
    private List<SlotRewardView> _slots;

    private bool _rewardReceived = false;

    public WeekRewardController(RewardView rewardView)
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
        if (_rewardView.LastRewardWeekTime.HasValue)
        {
            var timeSpan = DateTime.UtcNow - _rewardView.LastRewardWeekTime.Value;
            if (timeSpan.Seconds > _rewardView.TimeWeekDeadline)
            {
                _rewardView.LastRewardWeekTime = null;
                _rewardView.CurrentActiveWeekSlot = 0;
            }
            else if (timeSpan.Seconds < _rewardView.TimeWeekCooldown)
            {
                _rewardReceived = true;
            }
        }
    }

    private void RefreshUi()
    {
        _rewardView.GetRewardButton.interactable = !_rewardReceived;

        for (var i = 0; i < _rewardView.WeekRewards.Count; i++)
        {
            _slots[i].SetData(_rewardView.WeekRewards[i], i + 1, i <= _rewardView.CurrentActiveWeekSlot);
        }

        DateTime nextWeekBonusTime =
            !_rewardView.LastRewardWeekTime.HasValue
                ? DateTime.MinValue
                : _rewardView.LastRewardWeekTime.Value.AddSeconds(_rewardView.TimeWeekCooldown);
        var delta = nextWeekBonusTime - DateTime.UtcNow;
        if (delta.TotalSeconds < 0)
            delta = new TimeSpan(0);

        _rewardView.RewardWeekTimer.text = delta.ToString();
    }

    private void InitSlots()
    {
        _slots = new List<SlotRewardView>();
        for (int i = 0; i < _rewardView.WeekRewards.Count; i++)
        {
            var reward = _rewardView.WeekRewards[i];
            var slotInstance = GameObject.Instantiate(_rewardView.SlotPrefab, _rewardView.SlotsParent, false);
            slotInstance.SetData(reward, i + 1, false);
            _slots.Add(slotInstance);
        }
    }

    private void SubscribeButtons()
    {
        _rewardView.ResetButton.onClick.AddListener(ResetReward);
        _rewardView.GetWeekRewardButton.onClick.AddListener(() => ClaimReward(RewardDateTime.Week));
    }

    private void ResetReward()
    {
        _rewardView.LastRewardWeekTime = null;
        _rewardView.CurrentActiveWeekSlot = 0;
    }

    private void ClaimReward(RewardDateTime rewardDateTime)
    {
        if (_rewardReceived)
        {
            return;
        }

        var reward = _rewardView.WeekRewards[_rewardView.CurrentActiveWeekSlot];
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

        _rewardView.LastRewardWeekTime = DateTime.UtcNow;
        _rewardView.CurrentActiveWeekSlot = (_rewardView.CurrentActiveWeekSlot + 1) % _rewardView.WeekRewards.Count;
        RefreshRewardState();
    }
}
