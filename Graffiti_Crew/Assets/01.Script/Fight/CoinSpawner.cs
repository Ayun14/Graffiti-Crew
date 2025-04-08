using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private float _width, _height;
    [Range(1, 10)][SerializeField] private float _forcePower;

    private Vector3 _playerCoinSpawnPos;
    private Vector3 _rivalCoinSpawnPos;

    private void Awake()
    {
        _playerCoinSpawnPos = transform.Find("PlayerCoinSpawnPos").GetComponent<Transform>().position;
        _rivalCoinSpawnPos = transform.Find("RivalCoinSpawnPos").GetComponent<Transform>().position;
    }

    private void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnCoinToPlayer();
            SpawnCoinToRival();
        }
    }

    public void SpawnCoinToPlayer()
    {
        StartCoroutine(SpawnCoin(_playerCoinSpawnPos, 10));
    }

    public void SpawnCoinToRival()
    {
        StartCoroutine(SpawnCoin(_rivalCoinSpawnPos, 10));
    }

    private IEnumerator SpawnCoin(Vector3 spawnTrm, int spawnNum)
    {
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

            yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 size = new Vector3(_width, _height, 1);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
