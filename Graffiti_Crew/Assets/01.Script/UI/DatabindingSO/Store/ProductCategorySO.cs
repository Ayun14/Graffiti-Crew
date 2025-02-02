using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "ProductCategorySO", menuName = "SO/UI/Store/ProductCategorySO")]
    public class ProductCategorySO : ScriptableObject
    {
        public ProductSO[] products;
    }
}