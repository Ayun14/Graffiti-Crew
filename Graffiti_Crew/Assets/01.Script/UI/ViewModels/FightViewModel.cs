using AH.UI.Models;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class FightViewModel : ViewModel {
        private FightModel _model;
        public FightViewModel(Model model) {
            _model = model as FightModel;
        }
    }
}