using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "LoadStageSO", menuName = "SO/Map/LoadStageSO")]
    public class LoadStageSO : SaveDataSO, IResetData {
        [Space]
        public string chapter;
        public string stage;

        [SerializeField]private string _currentStageName;
        [SerializeField] private StageType _currentStageType;
        [Space]
        [SerializeField] private string _defaultChapter;
        [SerializeField] private string _defaultStage;

        private void Awake() {
            dataType = DataType.Ect;
        }

        public string GetLoadStageName() {
            return $"{chapter}/{stage}";
        }
        public override string GetData() {
            return $"{chapter} {stage}";
        }
        public void SetCurrentStage(string stage, StageType type) {
            _currentStageName = $"{stage}";
            _currentStageType = type;
        }
        public string GetCurrentStageName() {
            return _currentStageName;
        }
        public StageType GetCurrentStageType() {
            return _currentStageType;
        }
        public override void SetValueFromString(string value) {
            string[] datas = value.Split(" ");
            chapter = datas[0];
            stage = datas[1];
        }
        public override string GetDataType() {
            return dataType.ToString();
        }
        public override void ResetData() {
            chapter = _defaultChapter;
            stage = _defaultStage;
        }
    }
}