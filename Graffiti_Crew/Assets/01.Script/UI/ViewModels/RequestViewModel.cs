using AH.UI.Models;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class RequestViewModel : ViewModel {
        private RequestModel _model;
        public RequestViewModel(Model model) {
            _model = model as RequestModel;
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
        public override void SetVFXValue(int value) {
            _model.SetVFXValue(value);
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
