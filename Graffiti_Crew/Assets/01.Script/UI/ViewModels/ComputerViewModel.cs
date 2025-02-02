using AH.UI.Data;
using AH.UI.Models;
using System;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;

        public ComputerViewModel(ComputerModel model) {
            _model = model;
            UpdateDisplay(0); // 초기 데이터를 View에 반영
        }

        #region GetData
        public CrewSO GetCrew() {
            return _model.GetCrew();
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _model.GetExpeditionMember();
        }
        public ProductDescriptionSO GetProductDescription() {
            return _model.GetProductDescription();
        }
        public CategoryListSO GetCategory() {
            return _model.GetCategory();
        }
        public InputReaderSO GetInputReader() {
            return _model.GetInputReader();
        }
        #endregion

        public void SetFriendImg(int btnIndex, int friendIndex) {
            if(btnIndex < 0) {
                Debug.LogWarning("문제 생김");
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

    }
}
