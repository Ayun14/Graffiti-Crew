using UnityEngine;

namespace AH.Save {
    [CreateAssetMenu(fileName = "ItemSaveDataSO", menuName = "SO/Save/Data/ItemSaveDataSO")]
    public class ItemSaveDataSO : SaveDataSO {
        [Space]
        public string itemName;
        public int count;
        [Space]
        [SerializeField] private string _defaultItemNameData;
        [SerializeField] private int _defaultStarData = 0;

        private void Awake() {
            dataType = DataType.Item;
        }
        public override string GetDataType() {
            return dataType.ToString();
        }
        public override string GetData() {
            return $"{itemName}:{count}";
        }
        public override void SetValueFromString(string value) {
            string[] datas = value.Split(":");
            itemName = datas[0];
            count = int.Parse(datas[1]);
        }
        public override void ResetData() {
            itemName = _defaultItemNameData;
            count = _defaultStarData;
        }
    }
}