using System.Collections.Generic;
using UnityEngine;


namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataListSO", menuName = "SO/Save/SaveDataListSO")]
    public class SaveDataListSO : ScriptableObject
    {
        public string saveFileName = "SaveFile";
        public List<SaveDataSO> saveDataSOList = new List<SaveDataSO>();

        [System.Serializable]
        private class SaveDataSerialized {
            public string dataName;
            public int ID;
            public string dataType;
            public string data;
        }

        [System.Serializable]
        private class SaveDataWrapper {
            public List<SaveDataSerialized> saveDataList = new List<SaveDataSerialized>();
        }

        public string ToJson() {
            SaveDataWrapper wrapper = new SaveDataWrapper();

            foreach (var saveData in saveDataSOList) {
                wrapper.saveDataList.Add(new SaveDataSerialized {
                    dataName = saveData.dataName,
                    ID = saveData.ID,
                    dataType = saveData.GetDataType(),
                    data = saveData.GetData()
                });
            }

            return JsonUtility.ToJson(wrapper, true); // PrettyPrint
        }
        public void LoadJson(string json) {
            SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);
            foreach (var data in wrapper.saveDataList) {
                // 동일한 SO를 찾기
                SaveDataSO findData = saveDataSOList.Find(currentData => currentData.ID == data.ID && currentData.dataName == data.dataName);
                if (findData != null) {
                    findData.SetValueFromString(data.data);
                }
            }
        }

        public void ResetDatas() {
            foreach (var data in saveDataSOList) {
                data.ResetData();
            }
        }
    }
}