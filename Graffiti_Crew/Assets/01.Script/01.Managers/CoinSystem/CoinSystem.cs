using UnityEngine;

public class CoinSystem : MonoBehaviour {
    private static int _coin;

    public static int GetCoin() {
        return _coin;
    } 
    public static void SetCoin(int coin) {
        _coin = coin;
    }
    public static void AddCoin(int coin) {
        _coin += coin;
    }
    public static void MinusCoin(int coin) {
        if (coin < 0) {
            return;
        }

        if(_coin - coin >= 0) {
            _coin -= coin;
        }
        else {
            Debug.LogError("µ∑¿Ã -ø° ");
        }
    }
}
