using AH.UI.ViewModels;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class NotEnoughView : UIView {
        private bool _isShowing = false;
        public NotEnoughView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Show() {
            if (!_isShowing) {
                _isShowing = true;
                topElement.AddToClassList("show");
                Hide();
            }
            Debug.Log("show");
            base.Show();
        }
        public async override void Hide() {
            //if (_isShowing) {
                await Task.Delay(1500);
                topElement.RemoveFromClassList("show");
                await Task.Delay(400);
                Debug.Log("hide");
                base.Hide();
                _isShowing = false;
            //}
        }
    }
}
