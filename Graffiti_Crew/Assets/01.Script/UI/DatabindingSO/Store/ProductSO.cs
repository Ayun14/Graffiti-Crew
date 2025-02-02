using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "ProductSO", menuName = "SO/UI/Store/ProductSO")]
    public class ProductSO : ScriptableObject
    {
        public string itemName;
        public string description;
        public float price;
        public Sprite image;
    }
}