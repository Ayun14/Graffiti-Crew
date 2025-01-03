using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/FloatSaveDataSO")]
    public class FloatSaveDataSO : SaveDataSO {
        public float data;
        private float _defaultData;

        private void Awake() {
            _defaultData = data;
        }
        public override string GetDataType() {
            return dataType.ToString();
        }
        public override string GetData() {
            return data.ToString();
        }
        public override void SetValueFromString(string value) {
            data = float.Parse(value);
        }

        public override void ResetData() {
            data = _defaultData;
        }
    }
}
