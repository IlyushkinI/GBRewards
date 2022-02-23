using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerRewardDataHandler : MonoBehaviour
    {
        #region Fields

        public readonly string SaveFolder = @"/Saves/";
        public readonly string SaveFileName = @"PlayerData.json";

        private PlayerRewardData _currentPlayerRewardData;

        #endregion

        #region Properties

        public int WoodCount
        {
            get => _currentPlayerRewardData.WoodCount;
            set
            {
                _currentPlayerRewardData.WoodCount = value;
                SaveData();
            }
        }

        public int DimondCount
        {
            get => _currentPlayerRewardData.DiamondCount;
            set
            {
                _currentPlayerRewardData.DiamondCount = value;
                SaveData();
            }
        }

        public int ActiveSlotKeyDaily
        {
            get => _currentPlayerRewardData.ActiveSlotKeyDaily;
            set
            {
                _currentPlayerRewardData.ActiveSlotKeyDaily = value;
                SaveData();
            }
        }

        public DateTime? LastTimeKeyDaily
        {
            get
            {
                if (string.IsNullOrEmpty(_currentPlayerRewardData.LastTimeKeyDaily))
                {
                    SaveData();
                    return null;
                }
                return DateTime.Parse(_currentPlayerRewardData.LastTimeKeyDaily);
            }
            set
            {
                if (value != null)
                {
                    _currentPlayerRewardData.LastTimeKeyDaily = value.ToString();
                    SaveData();
                }
                else
                {
                    _currentPlayerRewardData.LastTimeKeyDaily = default;
                    SaveData();
                }
            }
        }

        #endregion

        #region UnityMethods

        private void Awake()
        {
            if (!Directory.Exists(Application.dataPath + SaveFolder))
            {
                Directory.CreateDirectory(Application.dataPath + SaveFolder);
                File.Create(Application.dataPath + SaveFolder + SaveFileName);
                _currentPlayerRewardData = new PlayerRewardData();
                SaveData();
            }
            else
            {
                _currentPlayerRewardData = LoadData();
                SaveData();
            }
            
        }

        private void OnDestroy()
        {
            SaveData();
        }

        #endregion

        #region Methods

        public void SaveData()
        {
            var jsonString = JsonUtility.ToJson(_currentPlayerRewardData);
            File.WriteAllText(Application.dataPath + SaveFolder + SaveFileName, jsonString);
        }

        public PlayerRewardData LoadData()
        {
            if (_currentPlayerRewardData is null)
            {
                var jsonString = File.ReadAllText(Application.dataPath + SaveFolder + SaveFileName);
                var data = JsonUtility.FromJson<PlayerRewardData>(jsonString);
                if (data is null)
                    return InitDefault();
                else return data;
            }
            else return _currentPlayerRewardData;
        }

        private PlayerRewardData InitDefault()
        {
            return new PlayerRewardData
            {
                LastTimeKeyDaily = default,
                LastTimeKeyWeekly = default,
                ActiveSlotKeyDaily = default,
                ActiveSlotKeyWeekly = default,
                WoodCount = default,
                DiamondCount = default
            };
        }

        #endregion
    }
}
