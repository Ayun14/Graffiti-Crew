using AH.UI.Models;
using System;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class FightViewModel : ViewModel {
        private FightModel _model;
        public FightViewModel(Model model) {
            _model = model as FightModel;
        }

        public string GetStageName() {
            return _model.GetStageName();
        }

        public StageType GetStageType() {
            return _model.GetStageType();
        }

        public SliderValueSO GetSprayData() {
            return _model.GetSprayData();
        }
    }
}