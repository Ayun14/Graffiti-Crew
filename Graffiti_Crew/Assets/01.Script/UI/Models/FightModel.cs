using AH.SaveSystem;
using System;
using UnityEngine;

namespace AH.UI.Models {
    public class FightModel : Model {
        [SerializeField] private LoadStageSO _loadStageSO;
        [SerializeField] private SliderValueSO _sprayGauage;

        [Header("Setting")]
        [SerializeField] private LanguageSO _languageSO;
        [SerializeField] private IntSaveDataSO _languageIndexSO;
        [SerializeField] private IntSaveDataSO _bgmData;
        [SerializeField] private IntSaveDataSO _vfxData;
        public string GetStageName() {
            return _loadStageSO.GetCurrentStageName();
        }
        public StageType GetStageType() {
            return _loadStageSO.GetCurrentStageType();
        }
        public SliderValueSO GetSprayData() {
            return _sprayGauage;
        }

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