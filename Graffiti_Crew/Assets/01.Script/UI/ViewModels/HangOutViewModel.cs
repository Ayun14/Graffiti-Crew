using AH.UI.Models;
using System;

namespace AH.UI.ViewModels {
    public class HangOutViewModel : ViewModel{
        private HangOutModel _model;
        public HangOutViewModel(Model model) {
            _model = model as HangOutModel;
        }

        public bool IsPlayTutorial() {
            return _model.GetTutorialCheck().data;
        }
    }
}