using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace AH.SaveSystem {
    [CreateAssetMenu(fileName = "LoadStageSO", menuName = "SO/Map/LoadStageSO")]
    public class LoadStageSO : SaveDataSO, IResetData {
        [Space]
        public string chapter;
        public string stage;

        public string requestChapter;
        public string requestStage;

        public string stroyChapter;
        public string stroyStage;
        [Space]
        [SerializeField] private string _defaultChapter;
        [SerializeField] private string _defaultStage;
        [SerializeField] private string _defaultRequestChapter;
        [SerializeField] private string _defaultRequestStage;
        [SerializeField] private string _defaultStroyChapter;
        [SerializeField] private string _defaultStroyStage;

        private void Awake() {
            dataType = DataType.Ect;
        }

        public string GetLoadStageName() {
            return $"{chapter}/{stage}";
        }
        public string GetLoadRequestName() {
            return $"{requestChapter}/{requestStage}";
        }
        public string GetLoadStoryName() {
            return $"{stroyChapter}/{stroyStage}";
        }
        public override string GetData() {
            return $"{chapter} {stage} {requestChapter} {requestStage}";
        }
        public override void SetValueFromString(string value) {
            string[] datas = value.Split(" ");
            chapter = datas[0];
            stage = datas[1];
            requestChapter = datas[2];
            requestStage = datas[3];
            stroyChapter = datas[4];
            stroyStage = datas[5];
        }
        public override string GetDataType() {
            return dataType.ToString();
        }
        public override void ResetData() {
            chapter = _defaultChapter;
            stage = _defaultStage;
            requestChapter = _defaultRequestChapter;
            requestStage = _defaultRequestStage;
            stroyChapter = _defaultStroyChapter;
            stroyStage = _defaultStroyStage;
        }
    }
}