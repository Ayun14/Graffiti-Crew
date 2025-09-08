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
            GameManager.SetSlot(); // ���� ����
            CreateSlotData();
        }
        private void CreateSlotData() {
            // ���� �� ��ü ������ �Ѹ���
            SetData(currentSlot, slotGameDataList);
            LoadData(currentSlot, slotGameDataList);

            // �� �� �� ������
            SetData(currentSlot, _currentSceneLoadDataList);
            LoadData(currentSlot, _currentSceneLoadDataList);
        }

        // �ʱ� ���� ����
        private void SetData(SlotSO slot, List<SaveDataListSO> saveList) {
            FileSystem.CheckToSlotFolder(slot.slotName); // ������ ������ ����
            foreach (var saveData in saveList) { // save���Ͽ� �⺻�� �ֱ�
                if (!FileSystem.CheckToSaveFile(slot.slotName, saveData.saveFileName)) {
                    foreach (IResetData data in saveData.saveDataSOList) {
                        data.ResetData();
                    }
                    string jsonFile = saveData.ToJson();
                    FileSystem.WriteToFile(slot.slotName, saveData.saveFileName, jsonFile);
                }
            }
        }
        // file���� �������� so�� �ֱ�
        private void LoadData(SlotSO slot, List<SaveDataListSO> saveList) {
            foreach (var saveData in saveList) { // ������ load�ϱ�
                if (FileSystem.LoadFromFile(slot.slotName, saveData.saveFileName, out var jsonString)) {
                    saveData.LoadJson(jsonString);
                }
            }
            SaveDataEvents.LoadEndEvent?.Invoke();
        }
        public void SaveGameData(string sceneName = "") { // ��� �����͸� ����
            SetLastPlayTime();

            foreach (var saveData in _shareDataList) { // ���� ����
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(_shareSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
            foreach (var saveData in slotGameDataList) { // �����ε� �׻�ε� �Ǵ� ������
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
            foreach (var saveData in _currentSceneLoadDataList) { // ���� ���� ����
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

            _lastPlayTimeSO.data = $"{year}/{month}/{day}  {hour}�� {minute}��";
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
