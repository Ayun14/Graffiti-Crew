using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/IntSaveDataSO")]

    public class IntSaveDataSO : SaveDataSO{
        public int data;
        private int _defaultData;

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
            data = int.Parse(value);
        }

        public override void ResetData() {
            data = _defaultData;
        }
    }
}