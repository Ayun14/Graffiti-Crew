using AH.UI.Command;
using AH.UI.Models;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;

        // View에 표시할 데이터를 ReactiveProperty로 정의
        public ReactiveProperty<string> DisplayMessage { get; private set; } = new ReactiveProperty<string>();

        public ComputerViewModel(ComputerModel model) {
            _model = model;
            UpdateDisplay(); // 초기 데이터를 View에 반영
        }

        // 사용자 이름 설정 명령 
        public ICommand SetUserNameCommand => new UserNameCommand(this);

        // 사용자 이름 설정 메서드
        public void SetUserName(string name) {
            // Model에 데이터 저장
            _model.SetUserName(name);

            // 상태 변경 후 메시지 갱신
            UpdateDisplay();
        }

        public VisualTreeAsset GetStagePointAsset() {
            return _model.GetStagePointAsset();
        }

        public List<Transform> GetChapter1Ratio() {
            return _model.GetChapter1Ratio();
        }

        // DisplayMessage를 갱신하여 View에 알림
        private void UpdateDisplay() {
            DisplayMessage.Value = _model.FetchGreetingMessage();
        }

        // Command 클래스: View에서 발생한 명령을 처리
        private class UserNameCommand : ICommand {
            private ComputerViewModel viewModel;

            public UserNameCommand(ComputerViewModel vm) {
                viewModel = vm;
            }

            public void Execute(string parameter) {
                // View에서 전달받은 데이터를 ViewModel의 메서드에 전달
                viewModel.SetUserName(parameter);
            }
        }
    }
}
