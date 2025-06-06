using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "CrewSO", menuName = "SO/UI/Crew/CrewSO")]
    public class CrewSO : ScriptableObject
    {
        public MemberDataSO[] crew;
        public Sprite GetProfile(int index) {
            return crew[index].profile;
        }
    }
}
