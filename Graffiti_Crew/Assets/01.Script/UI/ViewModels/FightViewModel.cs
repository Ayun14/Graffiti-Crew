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

        public LanguageSO GetLanguageSO() {
            return _model.GetLanguageSO();
        }
        public int GetLanguageIndex() {
            return _model.GetLanguageIndex();
        }
        public void SetLanguageIndex(int index) {
            _model.SetLanguageIndex(index);
        }
        public void SetBGMValue(int value) {
            _model.SetBGMValue(value);
        }
        public void SetVFXValue(int value) {
            _model.SetVFXValue(value);
        }
        public int GetBGMValue() {
            return _model.GetBGMValue();
        }
        public int GetVFXValue() {
            return _model.GetVFXValue();
        }
    }
}