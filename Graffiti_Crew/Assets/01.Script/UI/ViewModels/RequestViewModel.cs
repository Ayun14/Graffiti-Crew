using AH.UI.Models;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class RequestViewModel : ViewModel {
        private RequestModel _model;
        public RequestViewModel(Model model) {
            _model = model as RequestModel;
        }
    }
}
