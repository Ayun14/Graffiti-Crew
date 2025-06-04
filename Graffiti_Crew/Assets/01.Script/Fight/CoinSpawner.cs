using AH.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour, INeedLoding
{
    [Header("Pool")]
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private PoolTypeSO _coinPoolType;

    [Header("Coin")]
    [SerializeField] private float _width, _height;
    [SerializeField] private float _forcePower;

    private List<Transform> _spawnPosList = new();
    private Vector3 _playerCoinSpawnPos;
    private Vector3 _rivalCoinSpawnPos;

    private StageSaveDataSO _stageSaveDataSO;

    public void LodingHandle(DataController dataController)
    {
        _stageSaveDataSO = dataController.stageData.stageSaveData;

        dataController.SuccessGiveData();
    }

    public void Init(List<Transform> spawnPosList)
    {
        _spawnPosList = spawnPosList;
    }

    private void Awake()
    {
        _playerCoinSpawnPos = transform.Find("PlayerCoinSpawnPos").GetComponent<Transform>().position;
        _rivalCoinSpawnPos = transform.Find("RivalCoinSpawnPos").GetComponent<Transform>().position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SpawnCoinToRival();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SpawnCoinToPlayer();
        }
    }

    public void SpawnCoinToPlayer()
    {
        int star = _stageSaveDataSO.star;
        int coin = 0;
        if (0 < star) coin = 10 + (7 * star);
        StartCoroutine(SpawnCoin(_playerCoinSpawnPos, coin));
    }

    public void SpawnCoinToRival()
    {
        StartCoroutine(SpawnCoin(_rivalCoinSpawnPos, Random.Range(15, 25)));
    }

    private IEnumerator SpawnCoin(Vector3 spawnTrm, int spawnNum)
    {
        float baseWaitTime = 3f / spawnNum;
        for (int i = 0; i < spawnNum; ++i)
        {
            GameObject coinObj = _poolManager.Pop(_coinPoolType).GameObject;
            coinObj.transform.parent = transform;
            coinObj.transform.position = _spawnPosList[Random.Range(0, _spawnPosList.Count)].localPosition;

            float offsetX = Random.Range(-_width / 2f, _width / 2f);
            float offsetY = Random.Range(0f, _height); // 위쪽으로만 튀는 경우
            float offsetZ = Random.Range(-0.3f, 0.3f);
            Vector3 randomTarget = spawnTrm + new Vector3(offsetX, offsetY, offsetZ);

            Vector3 shootDir = (randomTarget - coinObj.transform.position).normalized;

            Rigidbody rigid = coinObj.GetComponent<Rigidbody>();
            rigid.AddForce(shootDir * _forcePower, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(baseWaitTime - 0.2f, baseWaitTime + 0.1f));
        }
    }
}
