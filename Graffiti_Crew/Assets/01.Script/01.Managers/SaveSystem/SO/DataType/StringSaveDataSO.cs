using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/StringSaveDataSO")]
    public class StringSaveDataSO : SaveDataSO {
        [Space]
        public string data;
        [Space]
        [SerializeField] private string _defaultData;

        private void Awake() {
            dataType = DataType.String;
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