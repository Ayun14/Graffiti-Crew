using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "SO/Save/IntSaveDataSO")]

    public class IntSaveDataSO : SaveDataSO{
        public int data;

        public override string GetDataType() {
            return dataType.ToString();
        }
        public override string GetData() {
            return data.ToString();
        }
        public override void SetValueFromString(string value) {
            data = int.Parse(value);
        }
    }
}
