using System;
using UnityEditor;
using UnityEngine;

namespace AH.Save {
    public enum DataType {
        Int,
        String,
        Float,
        Texture,
        Stage,
        Item,
        Ect,
        Bool
    }
    public abstract class SaveDataSO : ScriptableObject, IResetData // IResetData필요하면 사용 할 것
    {
        public string dataName;
        public int ID;
        public DataType dataType;

        public abstract string GetData();
        public abstract void SetValueFromString(string value);
        public abstract string GetDataType();
        public virtual void ResetData() {

        }
#if UNITY_EDITOR
        protected virtual void OnValidate() {
            ID = this.GetInstanceID();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
