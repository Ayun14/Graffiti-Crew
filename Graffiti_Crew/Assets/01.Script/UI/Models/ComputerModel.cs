using AH.UI.Data;
using UnityEngine;

namespace AH.UI.Models {
    public class ComputerModel : Model {
        [Header("Stage")]
        [SerializeField] private CrewSO crew;
        [SerializeField] private ExpeditionMemberSO _memberSO;

        [Header("Store")]
        [SerializeField] private ProductDescriptionSO _descriptionSO;
        [SerializeField] private ProductCategorySO _categorySO;

        public CrewSO GetCrew() {
            return crew;
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _memberSO;
        }
        public ProductDescriptionSO GetProductDescription() {
            return _descriptionSO;
        }
        public ProductCategorySO GetCategory() {
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
    }
}
