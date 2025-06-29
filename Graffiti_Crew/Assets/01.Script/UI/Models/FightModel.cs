using AH.SaveSystem;
using System;
using UnityEngine;

namespace AH.UI.Models {
    public class FightModel : Model {
        [SerializeField] private LoadStageSO _loadStageSO;
        [Space]
        [SerializeField] private SliderValueSO _sprayGauage;
        [SerializeField] private SliderValueSO _gameProgressGauage;
        [Space]
        [SerializeField] private Sprite[] _progressSprites;
        public LoadStageSO GetLoadStageSO() {
            return _loadStageSO;
        }
        public string GetStageName() {
            return _loadStageSO.GetCurrentStageName();
        }
        public StageType GetStageType() {
            return _loadStageSO.GetCurrentStageType();
        }
        public SliderValueSO GetSprayData() {
            return _sprayGauage;
        }

        public Sprite[] GetProgressSprites() {
            return _progressSprites;
        }
        public SliderValueSO GetGameProgressSO() {
            return _gameProgressGauage;
        }
    }
}