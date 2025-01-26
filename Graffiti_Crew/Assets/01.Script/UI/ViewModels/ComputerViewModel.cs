using AH.UI.Data;
using AH.UI.Models;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;

        public ComputerViewModel(ComputerModel model) {
            _model = model;
            UpdateDisplay(0); // �ʱ� �����͸� View�� �ݿ�
        }

        public CrewSO GetCrew() {
            return _model.GetCrew();
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _model.GetExpeditionMember();
        }
        public ProductDescriptionSO GetProductDescription() {
            return _model.GetProductDescription();
        }
        public ProductCategorySO GetCategory() {
            return _model.GetCategory();
        }

        // ����� �̸� ���� �޼���
        public void SetFriendImg(int btnIndex, int friendIndex) {
            if(btnIndex < 0) {
                Debug.LogWarning("���� ����");
                return;
            }
            Sprite image = _model.GetCrew().GetProfile(friendIndex);
            _model.SetMemderImg(btnIndex, image);
            UpdateDisplay(friendIndex);
        }
        private void UpdateDisplay(int friendIndex) {
            //Friend1Img.Value = _model.GetFriendData(friendIndex);
        }
    }
}
