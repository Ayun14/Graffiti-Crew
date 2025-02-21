using AH.SaveSystem;
using UnityEngine;

public class CoinSystem : MonoBehaviour {
    private static CoinSystem instance;
    [SerializeField] private IntSaveDataSO _coin;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    public static int GetCoin() {
        return instance._coin.data;
    } 
    public static void SetCoin(int coin) {
        instance._coin.data = coin;
    }
    public static void AddCoin(int coin) {
        instance._coin.data += coin;
    }
    public static bool MinusCoin(int coin) {
        if (coin < 0) {
            return false;
        }

        if(instance._coin.data - coin >= 0) {
            instance._coin.data -= coin;
            return true;
        }
        else {
            Debug.LogError("µ∑¿Ã -¿‘¥œ¥Ÿ");
            return false;
        }
    }
}
