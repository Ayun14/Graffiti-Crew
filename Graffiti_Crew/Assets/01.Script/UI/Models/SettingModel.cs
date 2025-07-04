using AH.Save;
using UnityEngine;

namespace AH.UI.Models {
    public class SettingModel : Model {
        [Header("Language")]
        [SerializeField] private LanguageSO _languageSO;
        [SerializeField] private IntSaveDataSO _languageIndexSO;
        [Header("Audio")]
        [SerializeField] private IntSaveDataSO _bgmData;
        [SerializeField] private IntSaveDataSO _sfxData;
 
        #region Language
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
        public void SetSFXValue(int value) {
            _sfxData.data = value;
        }
        public int GetBGMValue() {
            return _bgmData.data;
        }
        public int GetVFXValue() {
            return _sfxData.data;
        }
        #endregion
    }
}