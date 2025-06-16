using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
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
        }
        void OnDisable() {
            SaveDataEvents.SaveGameEvent -= SaveGameData;
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
            GameManager.SetSlot(); // ���� ����
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
        private void SetData(SlotSO slot, SaveDataListSO saveData) {
            Debug.Log(saveData.name);
            FileSystem.CheckToSlotFolder(slot.slotName); // ������ ������ ����
            if (!FileSystem.CheckToSaveFile(slot.slotName, saveData.saveFileName)) {
                foreach (IResetData data in saveData.saveDataSOList) {
                    data.ResetData();
                }
                string jsonFile = saveData.ToJson();
                FileSystem.WriteToFile(slot.slotName, saveData.saveFileName, jsonFile);
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
        private void LoadData(SlotSO slot, SaveDataListSO saveData) {
            if (FileSystem.LoadFromFile(slot.slotName, saveData.saveFileName, out var jsonString)) {
                Debug.Log($"LOAD : {saveData.saveFileName}");
                Debug.Log(jsonString);
                saveData.LoadJson(jsonString);
            }
            SaveDataEvents.LoadEndEvent?.Invoke();
        }

        public void SaveGameData(string sceneName = "") { // ��� �����͸� ����
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
                Debug.Log("load scene");
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
