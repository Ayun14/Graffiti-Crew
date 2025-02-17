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
    }
}