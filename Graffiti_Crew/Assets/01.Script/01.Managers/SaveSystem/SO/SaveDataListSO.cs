using System.Collections.Generic;
using UnityEngine;


namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataListSO", menuName = "SO/Save/SaveDataListSO")]
    public class SaveDataListSO : ScriptableObject
    {
        public string saveFileName = "SaveFile";
        [SerializeField] private List<SaveDataSO> _saveDataSOList = new List<SaveDataSO>();

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

            foreach (var saveData in _saveDataSOList) {
                wrapper.saveDataList.Add(new SaveDataSerialized {
                    dataName = saveData.dataName,
                    ID = saveData.ID,
                    dataType = saveData.GetDataType(),
                    data = saveData.GetValueAsString()
                });
            }

            return JsonUtility.ToJson(wrapper, true); // PrettyPrint
        }
        public void LoadJson(string json) {
            SaveDataWrapper wrapper = JsonUtility.FromJson<SaveDataWrapper>(json);

            foreach (var serializedData in wrapper.saveDataList) {
                // 동일한 SO를 찾기
                SaveDataSO existingData = _saveDataSOList.Find(data => data.ID == serializedData.ID && data.dataName == serializedData.dataName);

                if (serializedData != null) {
                    //Debug.Log($"{serializedData.dataName} : {serializedData.data}");
                    existingData.SetValueFromString(serializedData.data);
                }
                else {
                    Debug.LogError("알 수 없는 데이터");
                }
            }
        }
    }
}