using AH.UI.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AH.SaveSystem {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _dataList;
        public SlotSO currentSlot => GameManager.currentSlot;

        private void Awake() {
            //Init();
        }
        private void Start() {
            LoadGame();
        }
        public void Init() {
            GameObject root = GameObject.Find("SaveManager");
            if (root == null) {
                root = new GameObject { name = "SaveManager" };
                root.AddComponent<SaveSystem>();
                DontDestroyOnLoad(root);
            }
            else {
                DontDestroyOnLoad(root);
            }
        }
        void OnApplicationQuit() {
            SaveGameData();
        }

        public void LoadGame() {
            if (FileSystem.CheckToSlotFolder(currentSlot.slotName)) { // ������ ��� �����ߴٸ� 
                // save���Ͽ� �⺻�� �ֱ�
                foreach (var saveData in _dataList) {
                    string jsonFile = saveData.ToJson();
                    FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                }
            }
            foreach (var saveData in _dataList) { // ������ load�ϱ�
                if (FileSystem.LoadFromFile(currentSlot.slotName, saveData.saveFileName, out var jsonString)) {
                    saveData.LoadJson(jsonString);
                }
            }
        }
        public void SaveGameData() { // ��� �����͸� ����
            foreach (var saveData in _dataList) {   
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
        }
    }
}
