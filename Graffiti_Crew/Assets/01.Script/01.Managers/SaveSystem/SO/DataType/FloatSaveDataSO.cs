using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/FloatSaveDataSO")]
    public class FloatSaveDataSO : SaveDataSO {
        [Space]
        public float data;
        [Space]
        [SerializeField] private float _defaultData;

        private void Awake() {
            dataType = DataType.Float;
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
