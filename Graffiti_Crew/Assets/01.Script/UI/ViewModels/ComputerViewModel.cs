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
        public void SetStageData(string chapter, string stage, StageType stageType) {
            _model.SetStage(chapter, stage, stageType);
        }
        public void SetStoryData(string chapter, string stage, StageType stageType) {
            _model.SetStage(chapter, stage, stageType);
        }
        public void SetActivityData(string chapter, string stage, StageType stageType) {
            _model.SetStage(chapter, stage, stageType);
        }
        public string GetCurrentStageName() {
            return _model.GetLoadStage().GetCurrentStageName();
        }

        public bool HaveItem(ProductSO item) {
            return ItemSystem.CheckHaveItem(item);
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
