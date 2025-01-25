using AH.UI.Data;
using UnityEngine;

namespace AH.UI.Models {
    public class ComputerModel : Model {
        [Header("Members")]
        [SerializeField] private CrewSO crew;
        [SerializeField] private ExpeditionMemberSO _memberSO;

        public CrewSO GetCrew() {
            return crew;
        }
        public ExpeditionMemberSO GetExpeditionMember() {
            return _memberSO;
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
