using System.Collections;
using UnityEngine;

public class ReactionSpawner : Observer<GameStateController>
{
    [Header("Pool")]
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private PoolTypeSO _iconPoolType;

    [Header("View Pos")]
    [SerializeField] private Vector2 _xRange = new Vector2(0.1f, 0.9f); // X 범위 (뷰포트 비율)
    [SerializeField] private Vector2 _yRange = new Vector2(0.05f, 0.3f); // Y 범위 (뷰포트 비율)

    private int[] _indicesCache = null;

    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public void SpawnCoinToPlayer()
    {
        ////int coins = mySubject.IsPlayerWin ? _stageDataSO.stageResult.coin : 0;
        //StartCoroutine(SpawnCoin(_playerCoinSpawnPos, coins));

        StartCoroutine(SpawnIcon(10, mySubject.IsPlayerWin));
    }

    public void SpawnCoinToRival()
    {
        // StartCoroutine(SpawnCoin(_rivalCoinSpawnPos, Random.Range(10, 20)));

        StartCoroutine(SpawnIcon(10, !mySubject.IsPlayerWin));
    }

    private IEnumerator SpawnIcon(int spawnNum, bool isWin)
    {
        if (_indicesCache == null || _indicesCache.Length < spawnNum)
            _indicesCache = new int[spawnNum];

        for (int i = 0; i < spawnNum; i++)
            _indicesCache[i] = i;

        for (int i = 0; i < spawnNum; i++)
        {
            int rand = Random.Range(i, spawnNum);
            (_indicesCache[i], _indicesCache[rand]) = (_indicesCache[rand], _indicesCache[i]);
        }

        for (int i = 0; i < spawnNum; i++)
        {
            int index = _indicesCache[i];
            bool tempBool = Random.Range(0, 10) <= 1 ? !isWin : isWin;
            SpawnBubble(tempBool, index, spawnNum);
            yield return new WaitForSeconds(Random.Range(0f, 0.2f));
        }
    }

    private void SpawnBubble(bool isWin, int index, int total)
    {
        ReactionIcon reaction = _poolManager.Pop(_iconPoolType).GameObject.GetComponent<ReactionIcon>();
        reaction.transform.SetParent(transform);
        reaction.SetIconSprite(isWin);

        RectTransform rect = reaction.GetComponent<RectTransform>();

        float stepX = (_xRange.y - _xRange.x) / Mathf.Max(1, total - 1); // 균등 간격
        float baseX = _xRange.x + stepX * index;                          // 기본 위치
        float offset = stepX * 0.3f;                                      // 중앙 기준 ±30% 랜덤
        float randomX = baseX + Random.Range(-offset, offset);
        float randomY = Random.Range(_yRange.x, _yRange.y);

        rect.anchorMin = new Vector2(randomX, randomY);
        rect.anchorMax = rect.anchorMin;
        rect.anchoredPosition = Vector2.zero;

        reaction.PopIcon(0.3f);
    }

    public override void NotifyHandle() { }
}
