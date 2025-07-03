using UnityEngine;

namespace AH.Save {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/BoolSaveDataSO")]
    public class BoolSaveDataSO : SaveDataSO {
        [Space]
        public bool data;
        [Space]
        [SerializeField] private bool _defaultData = false;

        private void Awake() {
            dataType = DataType.Bool;
        }
        public override string GetData() {
            return data.ToString();
        }

        public override string GetDataType() {
            return dataType.ToString();
        }

        public override void SetValueFromString(string value) {
            data = bool.Parse(value);
        }

        public override void ResetData() {
            data = default;
            base.ResetData();
        }
    }
}