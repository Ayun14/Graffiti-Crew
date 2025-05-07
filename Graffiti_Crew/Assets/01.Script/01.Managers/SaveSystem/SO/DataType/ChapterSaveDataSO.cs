using Unity.VisualScripting;
using UnityEngine;

namespace AH.SaveSystem {
    public class ChapterSaveDataSO : SaveDataSO {
        public override string GetData() {
            return "test";
        }

        public override string GetDataType() {
            return dataType.ToString();
        }

        public override void SetValueFromString(string value) {

        }
        public override void ResetData() {

        }
    }
}
