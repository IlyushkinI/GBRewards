using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Root : MonoBehaviour
    {
        [SerializeField]
        private RewardView _rewardView;

        private DailyRewardController _controller;
        private PlayerRewardDataHandler _dataSaver;

        void Start()
        {
            _controller = new DailyRewardController(_rewardView);
        }
    }
}
