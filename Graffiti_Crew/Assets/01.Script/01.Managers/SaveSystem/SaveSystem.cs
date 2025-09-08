using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AH.Save {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _currentSceneLoadDataList;
        private List<SaveDataListSO> _shareDataList;
        private List<SaveDataListSO> slotGameDataList;

        [SerializeField] private StringSaveDataSO _lastPlayTimeSO;

        private SlotSO[] _slots;
        private string _slotPath = "UI/Setting/Slots/";

        private SlotSO _shareSlot;
        public SlotSO currentSlot {
            get => GameManager.currentSlot;

            set {
                GameManager.currentSlot = value;
            }
        }

        private void Awake() {
            _slots = Resources.LoadAll<SlotSO>(_slotPath);
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
            SetLastPlayTime();

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
        public string[] FindAllDataValue(string dataName, string fileName) {
            string[] values = new string[_slots.Length];

            for (int i = 0; i < _slots.Length; i++) {
                string slotName = _slots[i].slotName;
                string value = FileSystem.FindDataValue(slotName, fileName, dataName);
                values[i] = value;
            }

            return values;
        }
        private void SetLastPlayTime() {
            DateTime now = DateTime.Now;

            string formattedDateTime = now.ToString("yyyy-MM-dd HH:mm:ss");

            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int hour = now.Hour;
            int minute = now.Minute;

            _lastPlayTimeSO.data = $"{year}/{month}/{day}  {hour}시 {minute}분";
        }
        private void DeleteSaveData(int index) {
            FileSystem.DeleteFolder(_slots[index].slotName);
            CreateAndLoadData();
        }

        public static class SaveHelperSystem {
            private static SaveSystem _saveSystem;

            public static void SetSaveSystem(SaveSystem saveSystem) {
                _saveSystem = saveSystem;
            }
            public static string[] FindAllDataValue(string dataName, string fileName) {
                return _saveSystem.FindAllDataValue(dataName, fileName);
            }
        }
    }
}
