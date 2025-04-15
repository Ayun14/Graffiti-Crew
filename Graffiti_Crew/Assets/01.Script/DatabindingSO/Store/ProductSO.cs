using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "ProductSO", menuName = "SO/UI/Store/ProductSO")]
    public class ProductSO : ScriptableObject
    {
        public string saveName;
        public string itemName;
        public string description;
        public int price;
        public Sprite image;

        public bool BuyItem() {
            return CoinSystem.MinusCoin(price);
        }
        public bool BuyItem(int cnt) {
            return CoinSystem.MinusCoin(price * cnt);
        }
    }
}