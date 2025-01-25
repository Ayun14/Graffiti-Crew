using AH.UI.Data;
using AH.UI.Models;
using System.Collections.Generic;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;

        public ComputerViewModel(ComputerModel model) {
            _model = model;
            UpdateDisplay(0); // 초기 데이터를 View에 반영
        }

        public CrewSO GetCrew() {
            return _model.GetCrew();
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _model.GetExpeditionMember();
        }

        // 사용자 이름 설정 메서드
        public void SetFriendImg(int btnIndex, int friendIndex) {
            if(btnIndex < 0) {
                Debug.LogWarning("문제 생김");
                return;
            }
            Sprite image = _model.GetCrew().GetProfile(friendIndex);
            _model.SetMemderImg(btnIndex, image);
            UpdateDisplay(friendIndex);
        }
        // DisplayMessage를 갱신하여 View에 알림
        private void UpdateDisplay(int friendIndex) {
            //Friend1Img.Value = _model.GetFriendData(friendIndex);
        }
    }
}
