using Saves;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField]
    private RewardView _rewardView;

    private RewardController _controller;
    private SaveDataRepository _saveDataRepository;

    void Start()
    {
        _saveDataRepository = new SaveDataRepository();
        _saveDataRepository.Initialization();
        _controller = new RewardController(_rewardView, _saveDataRepository);
    }
}
