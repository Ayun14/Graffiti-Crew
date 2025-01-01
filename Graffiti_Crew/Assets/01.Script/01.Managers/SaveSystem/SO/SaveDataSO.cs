using System;
using UnityEditor;
using UnityEngine;

namespace AH.SaveSystem {
    public enum DataType {
        Int,
        String,
        Float,
        Texture
    }
    public abstract class SaveDataSO : ScriptableObject
    {
        public string dataName;
        public int ID;
        public DataType dataType;

        public abstract string GetData();
        public abstract void SetValueFromString(string value);
        public abstract string GetDataType();
#if UNITY_EDITOR
        protected virtual void OnValidate() {
            //string path = AssetDatabase.GetAssetPath(this);
            ID = this.GetInstanceID();
        }
#endif
    }
}
