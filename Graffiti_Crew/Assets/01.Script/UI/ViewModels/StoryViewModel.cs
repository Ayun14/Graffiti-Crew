using AH.UI.Models;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class StoryViewModel : ViewModel {
        private StoryModel _model;
        public StoryViewModel(Model model) {
            _model = model as StoryModel;
        }
    }
}