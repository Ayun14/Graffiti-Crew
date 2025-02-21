using AH.Map;
using AH.UI.Data;
using System;
using UnityEngine;
using VHierarchy.Libs;

namespace AH.UI.Models {
    public class ComputerModel : Model {
        [Header("Stage")]
        [SerializeField] private CrewSO crewSO;
        [SerializeField] private ExpeditionMemberSO _memberSO;

        [Header("Store")]
        [SerializeField] private CategoryListSO _categorySO;
        [SerializeField] private ProductDescriptionSO _descriptionSO;

        [Header("Map")]
        [SerializeField] private LoadStageSO _loadStageSO;

        public CrewSO GetCrew() {
            return crewSO;
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _memberSO;
        }
        public ProductDescriptionSO GetProductDescription() {
            return _descriptionSO;
        }
        public CategoryListSO GetCategory() {
            return _categorySO;
        }

        public void SetMemderImg(int index, Sprite sprite) {
            if (index > 2) {
                Debug.LogError("°³»ç°í");
                return; 
            }
            if (index == 0) {
                _memberSO.memder1Profile = sprite;
            }
            else if (index == 1) {
                _memberSO.memder2Profile = sprite;
            }
            else if (index == 2) {
                _memberSO.memder3Profile = sprite;
            }
        }
        public void SetSelectProduct(int categoryIndex, int index) {
            _descriptionSO.itemName = _categorySO.categoryList[categoryIndex].products[index].itemName;
            _descriptionSO.description = _categorySO.categoryList[categoryIndex].products[index].description;
            _descriptionSO.price = _categorySO.categoryList[categoryIndex].products[index].price;
            _descriptionSO.image = _categorySO.categoryList[categoryIndex].products[index].image;
        }
        public void ClearSelectProductData() {
            _descriptionSO.itemName = "";
            _descriptionSO.description = "";
            _descriptionSO.price = 0;
            _descriptionSO.image = null;
        }
        public void SetStage(string chapter, string stage) {
            _loadStageSO.chapter = chapter;
            _loadStageSO.stage = stage;
        }
    }
}
