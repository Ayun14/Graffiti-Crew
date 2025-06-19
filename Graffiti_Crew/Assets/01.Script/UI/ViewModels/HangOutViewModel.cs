using AH.UI.Models;

namespace AH.UI.ViewModels {
    public class HangOutViewModel : ViewModel{
        private HangOutModel _model;
        public HangOutViewModel(Model model) {
            _model = model as HangOutModel;
        }
    }
}