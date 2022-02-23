using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Root : MonoBehaviour
    {
        [SerializeField]
        private RewardView _rewardView;

        private RewardController _controller;
        private PlayerRewardDataHandler _dataSaver;

        void Start()
        {
            _controller = new RewardController(_rewardView);
        }
    }
}
