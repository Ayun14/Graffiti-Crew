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
        instance.PrintCurrentItem();
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
        instance.PrintCurrentItem();
    }
    public static void RemoveItem(ProductSO item, int count) {
        if (instance._itemDictionary.TryGetValue(item, out int val)) {
            if (val - count >= 1) {
                instance._itemDictionary[item] = val - count;
            }
            else {
                instance._itemDictionary.Remove(item);
                Debug.Log("remove");
            }
        }
        instance.PrintCurrentItem();
    }
    private void PrintCurrentItem() {
        Debug.Log(_itemDictionary.Count);
        foreach (KeyValuePair<ProductSO, int> entry in _itemDictionary) {
            Debug.Log("Key: " + entry.Key + ", Value: " + entry.Value);
        }
    }
    public static bool CheckTicket(AdmissionTicket[] tickets) {
        for (int i = 0; i < tickets.Length; i++) {
            if (instance._itemDictionary.TryGetValue(tickets[i].ticketType, out int val)) {
                if(tickets[i].count > val) {
                    Debug.Log("갯수 부족");
                    return false;
                }
                else { // 갯수가 모자라거나
                }
            }
            else { // 종류가 없거나
                Debug.Log("해당 종류가 없음");
                return false;
            }
        }
        for (int i = 0; i < tickets.Length; i++) { // 계산
            RemoveItem(tickets[i].ticketType, tickets[i].count);
        }
        // 입장 가능
        return true;
    }
}
