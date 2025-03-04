using AH.UI.Models;
using UnityEngine;


namespace AH.UI.ViewModels {
    public class HangOutViewModel : ViewModel{
        private HangOutModel _model;
        public HangOutViewModel(Model model) {
            _model = model as HangOutModel;
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
        public void SetBGMValue(int value)
        {
            _model.SetBGMValue(value);
        }
        public void SetVFXValue(int value)
        {
            _model.SetVFXValue(value);
        }
    }
}