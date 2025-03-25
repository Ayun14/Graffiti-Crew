using AH.SaveSystem;
using System;
using UnityEngine;

public enum StageType {
    Stage,
    Request,
    Story
}
namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "Chapter_Stage_", menuName = "SO/Save/Data/StageSaveDataSO")]
    public class StageSaveDataSO : SaveDataSO {
        [Space]
        public StageType stageType = StageType.Stage;
        public bool isClear;
        public int star;
        [Space]
        [SerializeField] private bool _defaultIsClearData = false;
        [SerializeField] private int _defaultStarData = 0;

        private void Awake() {
            dataType = DataType.Stage;
        }
        public override string GetDataType() {
            return dataType.ToString();
        }
        public override string GetData() {
            return $"{isClear} {star}";
        }
        public override void SetValueFromString(string value) {
            string[] datas = value.Split(" ");
            isClear = bool.Parse(datas[0]);
            star = int.Parse(datas[1]);
        }
        public override void ResetData() {
            isClear = _defaultIsClearData;
            star = _defaultStarData;
        }
    }
}