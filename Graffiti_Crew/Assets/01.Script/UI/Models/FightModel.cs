using AH.SaveSystem;
using System;
using UnityEngine;

namespace AH.UI.Models {
    public class FightModel : Model {
        [SerializeField] private LoadStageSO _loadStageSO;
        [SerializeField] private SliderValueSO _sprayGauage;

        public string GetChapter() {
            return _loadStageSO.chapter;
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
    }
}