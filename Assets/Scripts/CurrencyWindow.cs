using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class CurrencyWindow : MonoBehaviour
    {
        public static CurrencyWindow Instance { get; private set; }

        [SerializeField]
        private TextMeshProUGUI _diamondText;
        [SerializeField]
        private TextMeshProUGUI _woodText;
        [SerializeField]
        private PlayerRewardDataHandler _dataSaver;

        private void Start()
        {
            RefreshText();
        }

        private int Wood
        {
            get => _dataSaver.WoodCount;
            set => _dataSaver.WoodCount = value;
        }

        private int Diamond
        {
            get => _dataSaver.DimondCount;
            set => _dataSaver.DimondCount = value;
        }

        public void AddDiamond(int count)
        {
            Diamond += count;
            RefreshText();
        }

        public void AddWood(int count)
        {
            Wood += count;
            RefreshText();
        }

        private void RefreshText()
        {
            if (_diamondText != null)
                _diamondText.text = Diamond.ToString();
            if (_woodText != null)
                _woodText.text = Wood.ToString();
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

    }
}