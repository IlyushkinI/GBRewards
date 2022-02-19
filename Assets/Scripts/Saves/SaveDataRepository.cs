using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Saves
{
    public sealed class SaveDataRepository : ISaveDataRepository
    {
        private IData<SavedData> _data;
        private string _path;
        private const string _folderName = "dataSave";
        private const string _fileName = "data.bat";

        public void Initialization()
        {
            _data = new JsonData<SavedData>();
            _path = Path.Combine(Application.dataPath, _folderName);           
        }

        public void Save()
        {
            if (!Directory.Exists(Path.Combine(_path)))
            {
                Directory.CreateDirectory(_path);
            }
;
            var savedData = new SavedData()
            {
                RewardSave = new RewardSaveModel()
            };            

            _data.Save(savedData, Path.Combine(_path, _fileName));
            Debug.Log("Save");
        }

        public void Load()
        {
            var file = Path.Combine(_path, _fileName);

            if (!File.Exists(file))
            {
                return;
                //throw new DataException($"File {file} not found");
            }

            var savedData = _data.Load(file);
            savedData.RewardSave.InitSaveData();
            Debug.Log("Load");
        }
    }
}