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

        #region Setting
        public override LanguageSO GetLanguageSO() {
            return _model.GetLanguageSO();
        }
        public override int GetLanguageIndex() {
            return _model.GetLanguageIndex();
        }
        public override void SetLanguageIndex(int index) {
            _model.SetLanguageIndex(index);
        }
        public override void SetBGMValue(int value) {
            _model.SetBGMValue(value);
        }
        public override void SetSFXValue(int value) {
            _model.SetSFXValue(value);
        }
        public override int GetBGMValue() {
            return _model.GetBGMValue();
        }
        public override int GetVFXValue() {
            return _model.GetVFXValue();
        } 
        #endregion
    }
}