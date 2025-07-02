using AH.SaveSystem;
using AH.UI.Models;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.ViewModels {
    public class FightViewModel : ViewModel {
        private FightModel _model;

        public FightViewModel(Model model) {
            _model = model as FightModel;
        }

        public string GetStageName() {
            return _model.GetStageName();
        }

        public StageType GetStageType() {
            return _model.GetStageType();
        }
        public SliderValueSO GetSprayData() {
            return _model.GetSprayData();
        }

        public SliderValueSO GetGameProgressSO() {
            return _model.GetGameProgressSO();
        }

        public Sprite GetJiaImg() {
            Sprite[] sprites = _model.GetProgressSprites();
            return sprites[0];
        }
        public Sprite GetRivalImg() {
            Sprite[] sprites = _model.GetProgressSprites();
            LoadStageSO stageSO = _model.GetLoadStageSO();
            string chapterStr = stageSO.chapter.Replace("Chapter", ""); // "3"
            int chapterNum = int.Parse(chapterStr); // 3
            if (stageSO.stage.Contains("Activity")) {
                return null;
            }
            return sprites[chapterNum];
        }

        public LoadStageSO GetLoadStageSO() {
            return _model.GetLoadStageSO();
        }

        public Sprite GetRivalCheckImg() {
            Sprite[] sprites = _model.GetRivalCheckSprites();
            LoadStageSO stageSO = _model.GetLoadStageSO();
            string chapterStr = stageSO.chapter.Replace("Chapter", ""); // "3"
            int chapterNum = int.Parse(chapterStr); // 3
            return sprites[chapterNum - 1];
        }
    }
}