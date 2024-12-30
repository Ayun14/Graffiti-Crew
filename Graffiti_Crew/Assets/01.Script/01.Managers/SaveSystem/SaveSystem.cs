using System.Collections.Generic;
using UnityEngine;

namespace AH.SaveSystem {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _dataList;
        private void Awake() {
            LoadGame();
        }
        void OnApplicationQuit() {
            SaveGameData();
        }

        public void LoadGame() {
            foreach (var saveData in _dataList) {
                if (FileSystem.LoadFromFile(saveData.saveFileName, out var jsonString)) {
                    saveData.LoadJson(jsonString);
                }
            }
        }
        public void SaveGameData() { // ��� �����͸� ����
            foreach (var saveData in _dataList) {   
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(saveData.saveFileName, jsonFile);
            }
        }
    }
}
