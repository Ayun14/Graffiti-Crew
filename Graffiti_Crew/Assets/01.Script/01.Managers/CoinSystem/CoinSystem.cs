using AH.Save;
using System.Collections;
using UnityEngine;

public class CoinSystem : MonoBehaviour {
    private static CoinSystem instance;
    [SerializeField] private IntSaveDataSO _coinSO;
    private int _coin {
        get {
            return _coinSO.data;
        }
        set {
            _coinSO.data = value;
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    public static int GetCoin() {
        return instance._coin;
    } 
    public static void SetCoin(int coin) {
        instance._coin = coin;
    }
    public static void AddCoin(int coin) {
        instance._coin += coin;
    }
    public static bool MinusCoin(int minusCoin) {
        if (minusCoin < 0) {
            return false;
        }

        if(instance._coin - minusCoin >= 0) {
            instance.StartCoroutine(instance.CalculateCoin(minusCoin));
            return true;
        }
        else {
            Debug.LogError("���� -�Դϴ�");
            return false;
        }
    }

    private IEnumerator CalculateCoin(int price) {
        WaitForSeconds waitTime = new WaitForSeconds(0.05f);
        int halfPrice = price / 2; // ������ ���� ����
        int weight = price / 5; // �ʱ� ���ҷ�
        int decreaseAmount = Mathf.Max(1, weight / 10); 

        while (true) {
            if (price <= halfPrice) {
                decreaseAmount = Mathf.Max(1, weight / 10);
                weight = Mathf.Max(1, weight - decreaseAmount); // ���������� ����
                if(price - weight <= 0) {
                    weight = 1;
                }
            }

            price -= weight;
            _coin -= weight;

            if(price <= 0) {
                break;
            }
            yield return waitTime;
        }
    }
}
