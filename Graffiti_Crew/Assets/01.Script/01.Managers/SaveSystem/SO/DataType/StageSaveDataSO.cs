using AH.SaveSystem;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum StageType {
    Battle,
    Activity,
    Story,
    None
}
public enum StageState {
    Clear,
    CanPlay,
    Lock
}
namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "Chapter_Stage_", menuName = "SO/Save/Data/StageSaveDataSO")]
    public class StageSaveDataSO : SaveDataSO {
        [Space]
        public StageType stageType = StageType.Battle;
        [SerializeField] private StageState _stageState;
        public StageState stageState
        {
            get
            {
                return _stageState;
            }
            set
            {
                if(_stageState == StageState.Clear) return;
                _stageState = value;
            }
        }
        public int star;
        [Space]
        [SerializeField] private StageState _defaultIsClearData = StageState.Lock;
        [SerializeField] private int _defaultStarData = 0;

        private void Awake() {
            dataType = DataType.Stage;
        }
        public override string GetDataType() {
            return dataType.ToString();
        }
        public override string GetData() {
            return $"{_stageState} {star}";
        }
        public override void SetValueFromString(string value) {
            string[] datas = value.Split(" ");
            _stageState = (StageState)Enum.Parse(typeof(StageState), datas[0]);
            star = int.Parse(datas[1]);
        }
        public override void ResetData() {
            _stageState = _defaultIsClearData;
            star = _defaultStarData;
        }
    }
}