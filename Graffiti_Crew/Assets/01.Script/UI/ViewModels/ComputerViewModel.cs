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
        public CrewSO GetCrew() {
            return _model.GetCrew();
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _model.GetExpeditionMember();
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
        #endregion

        public void SetFriendImg(int btnIndex, int friendIndex) {
            if(btnIndex < 0) {
                Debug.LogWarning("¹®Á¦ »ý±è");
                return;
            }
            Sprite image = _model.GetCrew().GetProfile(friendIndex);
            _model.SetMemderImg(btnIndex, image);
            UpdateDisplay(friendIndex);
        }
        public void SetSelectProduct(int categoryIndex, int index) {
            _model.SetSelectProduct(categoryIndex, index);
        }
        public void ClearSelectProductData() {
            _model.ClearSelectProductData();
        }
        private void UpdateDisplay(int friendIndex) {
            //Friend1Img.Value = _model.GetFriendData(friendIndex);
        }
        public void SetStageData(string chapter, string stage) {
            _model.SetStage(chapter, stage);
        }
        public void SetRequest(string chapter, string stage) {
            _model.SetRequest(chapter, stage);
        }
    }
}
