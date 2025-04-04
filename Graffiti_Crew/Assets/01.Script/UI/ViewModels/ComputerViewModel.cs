using AH.UI.Data;
using AH.UI.Models;
using System;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;
        public int currentBtnIndex = -1;

        public ComputerViewModel(Model model) {
            _model = model as ComputerModel;
        }

        #region GetData
        public StageDescriptionSO GetStageDescription() {
            return _model.GetStageDescription();
        }
        public ProductDescriptionSO GetProductDescription() {
            return _model.GetProductDescription();
        }
        public CategoryListSO GetCategory() {
            return _model.GetCategory();
        }
        #endregion

        public void SetSelectProduct(int categoryIndex, int index) {
            _model.SetSelectProduct(categoryIndex, index);
        }
        public void ClearSelectProductData() {
            _model.ClearSelectProductData();
        }
        public void SetStageData(string chapter, string stage) {
            _model.SetStage(chapter, stage);
        }
        public void SetStoryData(string chapter, string stage) {
            _model.SetStoryData(chapter, stage);
        }
        public void SetRequestData(string chapter, string stage) {
            _model.SetRequest(chapter, stage);
        }
        public string GetCurrentStageName() {
            return _model.GetLoadStage().GetCurrentStageName();
        }
    }
}
