using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AH.SaveSystem {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _currentSceneLoadDataList;
        private List<SaveDataListSO> _shareDataList;
        private List<SaveDataListSO> slotGameDataList;

        private SlotSO _shareSlot;
        public SlotSO currentSlot {
            get => GameManager.currentSlot;

            set {
                GameManager.currentSlot = value;
            }
        }

        private void Awake() {
            _shareSlot = Resources.Load<SlotSO>("UI/Setting/ShareData");
            _shareDataList = Resources.LoadAll<SaveDataListSO>("DataList/Share").ToList();
            slotGameDataList = Resources.LoadAll<SaveDataListSO>("DataList/Slot").ToList();
        }
        private void Start() {
            CreateAndLoadData();
        }
        void OnEnable() {
            SaveDataEvents.SaveGameEvent += SaveGameData;
            SaveDataEvents.ChangeSlotEvent += CreateSlotData;
            SaveDataEvents.DeleteSaveDataEvent += DeleteSaveData;
        }

        void OnDisable() {
            SaveDataEvents.SaveGameEvent -= SaveGameData;
            SaveDataEvents.ChangeSlotEvent -= CreateSlotData;
            SaveDataEvents.DeleteSaveDataEvent -= DeleteSaveData;
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
        private void CreateAndLoadData() {
            SetData(_shareSlot, _shareDataList);
            LoadData(_shareSlot, _shareDataList);
            GameManager.SetSlot(); // 슬롯 세팅
            CreateSlotData();
        }
        private void CreateSlotData() {
            // 슬롯 별 전체 데이터 뿌리고
            SetData(currentSlot, slotGameDataList);
            LoadData(currentSlot, slotGameDataList);

            // 각 씬 별 데이터
            SetData(currentSlot, _currentSceneLoadDataList);
            LoadData(currentSlot, _currentSceneLoadDataList);
        }

        // 초기 파일 생성
        private void SetData(SlotSO slot, List<SaveDataListSO> saveList) {
            FileSystem.CheckToSlotFolder(slot.slotName); // 폴더가 없으면 생성
            foreach (var saveData in saveList) { // save파일에 기본값 넣기
                if (!FileSystem.CheckToSaveFile(slot.slotName, saveData.saveFileName)) {
                    foreach (IResetData data in saveData.saveDataSOList) {
                        data.ResetData();
                    }
                    string jsonFile = saveData.ToJson();
                    FileSystem.WriteToFile(slot.slotName, saveData.saveFileName, jsonFile);
                }
            }
        }
        // file값을 바탕으로 so에 넣기
        private void LoadData(SlotSO slot, List<SaveDataListSO> saveList) {
            foreach (var saveData in saveList) { // 데이터 load하기
                if (FileSystem.LoadFromFile(slot.slotName, saveData.saveFileName, out var jsonString)) {
                    saveData.LoadJson(jsonString);
                }
            }
            SaveDataEvents.LoadEndEvent?.Invoke();
        }
        public void SaveGameData(string sceneName = "") { // 모든 데이터를 저장
            foreach (var saveData in _shareDataList) { // 공용 저장
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(_shareSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
            foreach (var saveData in slotGameDataList) { // 개별인데 항상로드 되는 데이터
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
            foreach (var saveData in _currentSceneLoadDataList) { // 개별 파일 저장
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
            if(sceneName != "") {
                SceneManager.LoadScene(sceneName);
            }
        }
        private void DeleteSaveData() {
            FileSystem.DeleteFolder(currentSlot.slotName);
            CreateAndLoadData();
        }
    }
}
