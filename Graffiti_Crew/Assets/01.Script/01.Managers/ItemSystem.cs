using AH.UI.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour {
    private static ItemSystem instance;
    private Dictionary<ProductSO, int> _itemDictionary = new Dictionary<ProductSO, int>();

    private void Awake() {
        instance = this;
    }
    public static void AddItem(ProductSO item) {
        if (instance._itemDictionary.TryGetValue(item, out int val)) {
            instance._itemDictionary[item] = ++val;
        }
        else {
            instance._itemDictionary.Add(item, 1);
        }
    }
    public static void RemoveItem(ProductSO item) {
        if (instance._itemDictionary.TryGetValue(item, out int val)) {
            if (val > 1) {
                instance._itemDictionary[item] = --val;
            }
            else {
                instance._itemDictionary.Remove(item); 
            }
        }
    }
}
