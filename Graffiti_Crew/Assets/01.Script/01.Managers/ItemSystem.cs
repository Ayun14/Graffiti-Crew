using AH.SaveSystem;
using AH.UI.Data;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public class ItemSystem : MonoBehaviour {
    private static ItemSystem instance;

    public string directoryPath = "Assets/08.SO/SaveDatas/ItemData";
    [SerializeField] private SaveDataListSO saveItemList;

    private Dictionary<ProductSO, int> _itemDictionary = new Dictionary<ProductSO, int>();

    private void Awake() {
        instance = this;
        Init();
    }
    private void Init() {
        foreach (var data in saveItemList.saveDataSOList) {
            ProductSO item = Resources.Load($"Products/Sprays/{data.dataName}_ProductSO") as ProductSO;
            var conversionData = data as ItemSaveDataSO;
            instance._itemDictionary.Add(item, conversionData.count);
        }
    }
    public static void AddItem(ProductSO item) {
        if (instance._itemDictionary.TryGetValue(item, out int val)) {
            instance._itemDictionary[item] = ++val;
            instance.AddSaveItem(item);
        }
        else {
            instance._itemDictionary.Add(item, 1);
            instance.CreateItemSO(item);
        }
        instance.PrintCurrentItem();
    }
    public static void RemoveItem(ProductSO item, int count= 1) {
        if (instance._itemDictionary.TryGetValue(item, out int val)) {
            if (val - count >= 1) {
                instance._itemDictionary[item] = val - count;
                instance.RemoveSaveItem(item, count);
            }
            else {
                instance._itemDictionary.Remove(item);
                instance.RemoveSaveItem(item);
            }
        }
        instance.PrintCurrentItem();
    }

    private void CreateItemSO(ProductSO item, int count = 1) {
        ItemSaveDataSO newSO = ScriptableObject.CreateInstance<ItemSaveDataSO>();
        newSO.dataName = item.saveName;
        newSO.ID = newSO.GetInstanceID();
        newSO.itemName = item.itemName;
        newSO.count = count;

        saveItemList.saveDataSOList.Add(newSO);

        string assetPath = directoryPath + $"/{item.saveName}_Item.asset";
        assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        AssetDatabase.CreateAsset(newSO, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private void AddSaveItem(ProductSO item) {
        foreach(var data in saveItemList.saveDataSOList) {
            var conversionData = data as ItemSaveDataSO;
            if (conversionData.itemName == item.itemName) {
                conversionData.count++;
            }
        }
    }
    private void RemoveSaveItem(ProductSO item, int count = 1) {
        foreach (var data in saveItemList.saveDataSOList) {
            var conversionData = data as ItemSaveDataSO;
            if (conversionData.itemName == item.itemName) {
                conversionData.count -= count;
            }
        }
    }
    private void RemoveSaveItem(ProductSO item) {
        foreach (var data in saveItemList.saveDataSOList) {
            var conversionData = data as ItemSaveDataSO;
            if (conversionData.itemName == item.itemName) {
                saveItemList.saveDataSOList.Remove(conversionData);
                string path = $"{directoryPath}/{conversionData.name}.asset"; // 삭제할 SO 경로
                if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(path) != null) {
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                break;
            }
        }
    }

    private void PrintCurrentItem() {
        foreach (KeyValuePair<ProductSO, int> entry in _itemDictionary) {
            Debug.Log("Key: " + entry.Key + ", Value: " + entry.Value);
        }
    }
    public static bool CheckTicket(AdmissionTicket[] tickets) {
        for (int i = 0; i < tickets.Length; i++) {
            if (instance._itemDictionary.TryGetValue(tickets[i].ticketType, out int val)) {
                if(tickets[i].count > val) {
                    return false;
                }
                else { // 갯수가 모자라거나
                }
            }
            else { // 종류가 없거나
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
