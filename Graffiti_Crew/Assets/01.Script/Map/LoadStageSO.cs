using UnityEditor;
using UnityEngine;

namespace AH.Map {
    [CreateAssetMenu(fileName = "LoadStageSO", menuName = "SO/Map/LoadStageSO")]
    public class LoadStageSO : ScriptableObject {
        public string chapter;
        public string stage;
        public string GetLoadStageName() {
            return $"{chapter}/{stage}";
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }
}