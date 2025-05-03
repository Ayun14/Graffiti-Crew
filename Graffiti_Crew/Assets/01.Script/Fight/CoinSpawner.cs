using AH.SaveSystem;
using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour, INeedLoding
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _width, _height;
    [Range(1, 10)][SerializeField] private float _forcePower;

    private Vector3 _playerCoinSpawnPos;
    private Vector3 _rivalCoinSpawnPos;

    private StageSaveDataSO _stageSaveDataSO;

    public void LodingHandle(DataController dataController)
    {
        _stageSaveDataSO = dataController.stageData.stageSaveData;

        dataController.SuccessGiveData();
    }

    private void Awake()
    {
        _playerCoinSpawnPos = transform.Find("PlayerCoinSpawnPos").GetComponent<Transform>().position;
        _rivalCoinSpawnPos = transform.Find("RivalCoinSpawnPos").GetComponent<Transform>().position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnCoinToRival();
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
            float randX = Random.Range(spawnTrm.x - _width / 2, spawnTrm.x + _width / 2);
            float randY = Random.Range(spawnTrm.y - _height / 2, spawnTrm.y + _height / 2);
            Vector3 spawnPos = new Vector3(randX, randY, spawnTrm.z);
            GameObject coinObj = Instantiate(_coinPrefab, spawnPos, Quaternion.identity, transform);

            Rigidbody rigid = coinObj.GetComponent<Rigidbody>();
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.3f, 0.3f),
                Random.Range(0f, 0.3f),
                Random.Range(-0.3f, 0.3f)
            );
            Vector3 finalDirection = (transform.forward + randomOffset).normalized;
            rigid.AddForce(finalDirection * _forcePower, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(baseWaitTime - 0.2f, baseWaitTime + 0.1f));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 size = new Vector3(_width, _height, 1);
        Gizmos.DrawWireCube(transform.position + _playerCoinSpawnPos, size);
    }
}
