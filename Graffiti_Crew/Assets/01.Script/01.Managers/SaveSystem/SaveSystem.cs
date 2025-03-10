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
            FileSystem.CheckToSlotFolder(currentSlot.slotName); // 폴더가 없으면 생성
            // save파일에 기본값 넣기
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
            foreach (var saveData in _dataList) { // 데이터 load하기
                if (FileSystem.LoadFromFile(currentSlot.slotName, saveData.saveFileName, out var jsonString)) {
                    saveData.LoadJson(jsonString);
                }
            }
        }
        public void SaveGameData() { // 모든 데이터를 저장
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
