using AH.SaveSystem;
using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/StringSaveDataSO")]
    public class StringSaveDataSO : SaveDataSO {
        public string data;
        public override string GetDataType() {
            return dataType.ToString();
        }

        public override string GetValueAsString() {
            return data;
        }

        public override void SetValueFromString(string value) {
            data = value;
        }
    }
}