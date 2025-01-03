using AH.SaveSystem;
using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/StringSaveDataSO")]
    public class StringSaveDataSO : SaveDataSO {
        public string data;
        private string _defaultData;

        private void Awake() {
            _defaultData = data;
        }
        public override string GetDataType() {
            return dataType.ToString();
        }

        public override string GetData() {
            return data;
        }

        public override void SetValueFromString(string value) {
            data = value;
        }

        public override void ResetData() {
            data = _defaultData;
        }
    }
}