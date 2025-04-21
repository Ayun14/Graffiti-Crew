using AH.UI.Models;

namespace AH.UI.ViewModels {
    public class HangOutViewModel : ViewModel{
        private HangOutModel _model;
        public HangOutViewModel(Model model) {
            _model = model as HangOutModel;
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