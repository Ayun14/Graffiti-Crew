using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "CategoryListSO", menuName = "SO/UI/Store/CategoryListSO")]
    public class CategoryListSO : ScriptableObject {
        public ProductCategorySO[] categoryList;
    }
}