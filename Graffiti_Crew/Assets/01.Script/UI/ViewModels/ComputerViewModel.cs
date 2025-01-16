using AH.UI.Command;
using AH.UI.Models;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.ViewModels {
    public class ComputerViewModel : ViewModel {
        private ComputerModel _model;

        // View�� ǥ���� �����͸� ReactiveProperty�� ���� (Observer Pattern)
        public ReactiveProperty<string> DisplayMessage { get; private set; } = new ReactiveProperty<string>();

        public ComputerViewModel(ComputerModel model) {
            _model = model;
            UpdateDisplay(); // �ʱ� �����͸� View�� �ݿ�
        }

        // ����� �̸� ���� ��� (Command Pattern)
        public ICommand SetUserNameCommand => new UserNameCommand(this);

        // ����� �̸� ���� �޼���
        public void SetUserName(string name) {
            // Model�� ������ ����
            //_model.SetUserName(name);
            // ���� ���� �� �޽��� ���� (Observer Pattern)
            UpdateDisplay();
        }
        public VisualTreeAsset GetStagePointAsset() {
            return _model.GetStagePointAsset();
        }
        public List<Transform> GetChapter1Ratio() {
            return _model.GetChapter1Ratio();
        }

        // DisplayMessage�� �����Ͽ� View�� �˸� (Observer Pattern)
        private void UpdateDisplay() {
            //DisplayMessage.Value = _model.FetchGreetingMessage();
        }

        // Command Ŭ����: View���� �߻��� ����� ó��
        private class UserNameCommand : ICommand {
            private ComputerViewModel viewModel;

            public UserNameCommand(ComputerViewModel vm) {
                viewModel = vm;
            }

            public void Execute(string parameter) {
                // View���� ���޹��� �����͸� ViewModel�� �޼��忡 ����
                viewModel.SetUserName(parameter);
            }
        }
    }
}