using AH.SaveSystem;
using AH.UI.Data;
using System;
using UnityEngine;

namespace AH.UI.Models
{
    public class ComputerModel : Model
    {
        [Header("Stage")]
        [SerializeField] private StageDescriptionSO _stageDescription;

        [Header("Store")]
        [SerializeField] private CategoryListSO _categorySO;
        [SerializeField] private ProductDescriptionSO _descriptionSO;

        [Header("Map")]
        [SerializeField] private LoadStageSO _loadStageSO;

        public StageDescriptionSO GetStageDescription()
        {
            return _stageDescription;
        }
        public ProductDescriptionSO GetProductDescription()
        {
            return _descriptionSO;
        }
        public CategoryListSO GetCategory()
        {
            return _categorySO;
        }
        public LoadStageSO GetLoadStage() {
            return _loadStageSO;
        }

        public void SetSelectProduct(int categoryIndex, int index)
        {
            _descriptionSO.itemName = _categorySO.categoryList[categoryIndex].products[index].itemName;
            _descriptionSO.description = _categorySO.categoryList[categoryIndex].products[index].description;
            _descriptionSO.price = _categorySO.categoryList[categoryIndex].products[index].price;
            _descriptionSO.image = _categorySO.categoryList[categoryIndex].products[index].image;
        }
        public void ClearSelectProductData()
        {
            _descriptionSO.itemName = "";
            _descriptionSO.description = "";
            _descriptionSO.price = 0;
            _descriptionSO.image = null;
        }
        public void SetStage(string chapter, string stage)
        {
            _loadStageSO.chapter = chapter;
            _loadStageSO.stage = stage;
            _loadStageSO.SetCurrentStage(chapter + stage, StageType.Stage);
        }
        public void SetStoryData(string chapter, string stage) {
            _loadStageSO.stroyChapter = chapter;
            _loadStageSO.stroyStage = stage;
            _loadStageSO.SetCurrentStage(chapter + stage, StageType.Story);
        }
        public void SetRequest(string chapter, string stage)
        {
            _loadStageSO.requestChapter = chapter;
            _loadStageSO.requestStage = stage;
            _loadStageSO.SetCurrentStage(chapter + stage, StageType.Request);
        }
    }
}
