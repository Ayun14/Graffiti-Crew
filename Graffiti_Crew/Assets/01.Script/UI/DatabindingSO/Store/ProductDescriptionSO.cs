using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "ProductDescriptionSO", menuName = "SO/UI/Store/ProductDescriptionSO")]
    public class ProductDescriptionSO : ScriptableObject
    {
        public string itemName;
        public string description;
        public float price;
        public Sprite image;
    }
}