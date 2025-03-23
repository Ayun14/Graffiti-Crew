using AH.UI.Models;
using System;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class DialogViewModel : ViewModel {
        private DialogModel _model;
        public int currentBtnIndex = -1;

        public DialogViewModel(Model model) {
            _model = model as DialogModel;
        }
        public void SetProfile(Sprite profile) {
            _model.SetProfile(profile);
        }
    }
}