using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "ProductSO", menuName = "SO/UI/Store/ProductSO")]
    public class ProductSO : ScriptableObject
    {
        public string name;
        public string description;
        public Sprite icon;
        public float price;
    }
}