using UnityEngine;

namespace AH.UI.Models {
    public class HangOutModel : Model {
        [SerializeField] private LanguageSO _languageSO;

        public LanguageSO GetLanguageSO() {
            return _languageSO;
        }
    }
}
