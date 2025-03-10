using System.Collections.Generic;
using UnityEngine;

namespace AH.SaveSystem {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _dataList;
        public SlotSO currentSlot;// => GameManager.currentSlot;

        private void Awake() {
            //Init();
            CreateNewData();
        }
        void OnEnable() {
            GameEvents.SaveGameEvent += SaveGameData;
        }
        void OnDisable() {
            GameEvents.SaveGameEvent -= SaveGameData;
        }
        void OnApplicationQuit() {
            SaveGameData();
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
        private void CreateNewData() {
            FileSystem.CheckToSlotFolder(currentSlot.slotName); // ������ ������ ����
            // save���Ͽ� �⺻�� �ֱ�
            foreach (var saveData in _dataList) {
                if(!FileSystem.CheckToSaveFile(currentSlot.slotName, saveData.saveFileName)) {
                    Debug.Log(saveData.saveFileName);
                    string jsonFile = saveData.ToJson();
                    FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                }
            }
            LoadGame();
        }
        private void LoadGame() {
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
                //saveData.ResetDatas();
            }
        }
        private void ResetData() {

            CreateNewData();
        }
    }
}
