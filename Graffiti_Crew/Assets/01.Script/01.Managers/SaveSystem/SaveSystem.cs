using AH.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AH.SaveSystem {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _dataList;
        public SlotSO currentSlot;

        private void Awake() {
            LoadGame();
            Events.SelectSlotEvent += ChangeSlot;
        }

        private void ChangeSlot(string obj) {
            Debug.Log(obj);
        }

        void OnApplicationQuit() {
            SaveGameData();
        }

        public void LoadGame() {
            foreach (var saveData in _dataList) {
                if (FileSystem.CheckToSlotFolder(currentSlot.slotName)) {
                    string jsonFile = saveData.ToJson();
                    FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                }
                if(!FileSystem.LoadFromFile(currentSlot.slotName, saveData.saveFileName, out var jsonString)) {
                    saveData.LoadJson(jsonString);
                }
            }
        }
        public void SaveGameData() { // 모든 데이터를 저장
            foreach (var saveData in _dataList) {   
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
        }
    }
}
