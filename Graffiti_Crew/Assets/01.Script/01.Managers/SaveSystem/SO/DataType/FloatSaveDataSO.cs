using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/FloatSaveDataSO")]
    public class FloatSaveDataSO : SaveDataSO {
        public float data;

        public override string GetDataType() {
            return dataType.ToString();
        }
        public override string GetValueAsString() {
            return data.ToString();
        }
        public override void SetValueFromString(string value) {
            data = float.Parse(value);
        }
    }
}
