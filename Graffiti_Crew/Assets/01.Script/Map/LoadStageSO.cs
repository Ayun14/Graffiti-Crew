using UnityEngine;

namespace AH.Map {
    [CreateAssetMenu(fileName = "LoadStageSO", menuName = "SO/Map/LoadStageSO")]
    public class LoadStageSO : ScriptableObject {
        public string chapter;
        public string stage;
    }
}