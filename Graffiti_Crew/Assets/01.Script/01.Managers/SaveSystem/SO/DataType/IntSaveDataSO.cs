using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/Data/IntSaveDataSO")]

    public class IntSaveDataSO : SaveDataSO {
        [Space]
        public int data;
        [Space]
        [SerializeField] private int defaultData;

        private void Awake() {
            defaultData = data;
            dataType = DataType.Int;
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
            data = defaultData;
        }
    }
}