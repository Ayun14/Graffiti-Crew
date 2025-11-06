using AH.Save;
using AH.UI.Data;
using AH.UI.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;
        public int currentBtnIndex = -1;

        public ComputerViewModel(Model model) {
            _model = model as ComputerModel;
        }

        #region GetData
        public List<StageSaveDataSO> GetSaveStageDatas() {
            return _model.GetSaveStageDatas();
        }
        public StageDescriptionSO GetStageDescription() {
            return _model.GetStageDescription();
        }
        public ProductDescriptionSO GetProductDescription() {
            return _model.GetProductDescription();
        }
        public CategoryListSO GetCategory() {
            return _model.GetCategory();
        }
        public LoadStageSO GetLoadStageSO() {
            return _model.GetLoadStage();
        }
        #endregion

        public void SetSelectProduct(int categoryIndex, int index) {
            _model.SetSelectProduct(categoryIndex, index);
        }
        public void ClearSelectProductData() {
            _model.ClearSelectProductData();
        }
        public void SetStageData(string chapter, string stage, StageType stageType, string stageNumer) {
            _model.SetStage(chapter, stage, stageType, stageNumer);
        }
        public string GetCurrentStageName() {
            return _model.GetLoadStage().GetCurrentStageName();
        }

        public bool HaveItem(ProductSO item) {
            return ItemSystem.CheckHaveItem(item);
        }

        public Sprite GetDescriptionBackgroundImg() {
            return _model.GetDescriptionBackgroundImg();
        }
    }
}
