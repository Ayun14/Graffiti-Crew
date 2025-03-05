using AH.SaveSystem;
using UnityEngine;

namespace AH.UI.Models {
    public class HangOutModel : Model {
        [SerializeField] private LanguageSO _languageSO;
        [SerializeField] private IntSaveDataSO _languageIndexSO;

        [Header("Setting")]
        [SerializeField] private IntSaveDataSO _bgmData;
        [SerializeField] private IntSaveDataSO _vfxData;

        public LanguageSO GetLanguageSO() {
            return _languageSO;
        }
        public int GetLanguageIndex() {
            return _languageIndexSO.data;
        }
        public void SetLanguageIndex(int index) {
            _languageIndexSO.data = index;
        }
        public void SetBGMValue(int value) {
            _bgmData.data = value;
        }
        public void SetVFXValue(int value) {
            _vfxData.data = value;
        }
        public int GetBGMValue() {
            return _bgmData.data;
        }
        public int GetVFXValue() {
            return _vfxData.data;
        }
    }
}
