using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AH.SaveSystem {
    public class SaveSystem : MonoBehaviour {
        [Header("SaveDataLists")]
        [SerializeField] private List<SaveDataListSO> _dataList;
        List<SaveDataListSO> _shareDataList;

        private SlotSO _shareSlot;
        public SlotSO currentSlot {
            get => GameManager.currentSlot;

            set {
                GameManager.currentSlot = value;
            }
        }
        private void Awake() {
            _shareSlot = Resources.Load<SlotSO>("UI/Setting/ShareData");
            _shareDataList = Resources.LoadAll<SaveDataListSO>("ShareDataList").ToList();
            
        }
        private void Start() {
            CreateAndLoadData();
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
        private void CreateAndLoadData() {
            SetData(_shareSlot, _shareDataList);
            LoadData(_shareSlot, _shareDataList);
            GameManager.SetSlot();
            SetData(currentSlot, _dataList);
            LoadData(currentSlot, _dataList);
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
        }
        public void SaveGameData() { // ��� �����͸� ����
            foreach (var saveData in _shareDataList) { // ���� ����
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(_shareSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
            foreach (var saveData in _dataList) { // ���� ���� ����
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(currentSlot.slotName, saveData.saveFileName, jsonFile);
                saveData.ResetDatas();
            }
        }
    }
}
