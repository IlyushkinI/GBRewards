using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField]
    private RewardView _rewardView;

    private DailyRewardController _dailyController;
    private WeekRewardController _weekRewardController;

    void Start()
    {
        _dailyController = new DailyRewardController(_rewardView);
        _weekRewardController = new WeekRewardController(_rewardView);
    }
}
