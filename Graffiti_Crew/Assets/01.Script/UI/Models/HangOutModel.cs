using AH.SaveSystem;
using UnityEngine;

namespace AH.UI.Models {
    public class HangOutModel : Model {
        [SerializeField] private LanguageSO _languageSO;
        [SerializeField] private IntSaveDataSO _languageIndexSO;

        public LanguageSO GetLanguageSO() {
            return _languageSO;
        }
        public int GetLanguageIndex() {
            return _languageIndexSO.data;
        }
        public void SetLanguageIndex(int index) {
            _languageIndexSO.data = index;
        }
    }
}
