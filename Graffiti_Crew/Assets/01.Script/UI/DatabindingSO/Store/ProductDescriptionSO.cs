using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "ProductDescriptionSO", menuName = "SO/UI/Store/ProductDescriptionSO")]
    public class ProductDescriptionSO : ScriptableObject
    {
        public string name;
        public string description;
        public Sprite icon;
        public float price;
    }
}