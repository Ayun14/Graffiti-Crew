using AH.SaveSystem;
using AH.UI.Models;
using System;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class TitleViewModel : ViewModel
    {
        private TitleModel _model;

        public TitleViewModel(Model model) {
            _model = model as TitleModel;
        }
        #region SaveData
        public int GetSlotIndex() {
            return _model.GetSlotIndex();
        }
        public void SetSlotIndex(int index) {
            _model.SetSlotIndex(index);
        }
        #endregion
    }
}