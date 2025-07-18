using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudienceNPCController : Observer<GameStateController>
{
    [SerializeField] private GameObject _maleNCPPrefab;
    [SerializeField] private GameObject _femaleNCPPrefab;
    [Range(0, 14)][SerializeField] private int _minSpawnNum;
    [Range(0, 15)][SerializeField] private int _maxSpawnNum;

    private List<Transform> _spawnPosList = new();
    private CoinSpawner _coinSpawner;

    private void Awake()
    {
        Attach();

        _spawnPosList = transform.Find("SpawnPos").GetComponentsInChildren<Transform>().ToList();
        _spawnPosList.RemoveAt(0);

        _coinSpawner = GetComponent<CoinSpawner>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Finish)
            {
                AudienceNPCSpawn();

                AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.People_Idle);
                AnimationEvent.SetAnimation?.Invoke(11, AnimationEnum.People_Clap);
                AnimationEvent.SetAnimation?.Invoke(12, AnimationEnum.People_Pointing);
                AnimationEvent.SetAnimation?.Invoke(13, AnimationEnum.People_Reaction);
                AnimationEvent.SetAnimation?.Invoke(14, AnimationEnum.People_Talk);
            }
        }
    }

    private void AudienceNPCSpawn()
    {
        if (_coinSpawner.StageDataSO.stageType == StageType.Activity && mySubject.IsPlayerWin == false) return;

        int spawnNum = Random.Range(_minSpawnNum, _maxSpawnNum + 1);
        List<Transform> spawnPosList = new();
        for (int i = 0; i < spawnNum; ++i)
        {
            GameObject spawnGO = Random.Range(0, 2) == 0 ? _maleNCPPrefab : _femaleNCPPrefab;
            Spawn(spawnGO, _spawnPosList[i]);
            spawnPosList.Add(_spawnPosList[i]);
        }
        _coinSpawner.Init(spawnPosList);
    }

    private void Spawn(GameObject prefab, Transform spawnTrm)
    {
        GameObject go = Instantiate(prefab, spawnTrm);
        AnimationController animationController = go.GetComponentInChildren<AnimationController>();
        animationController.SetObjectID(Random.Range(10, 14));

        Transform visualTrm = go.transform.Find("Visual");
        if (visualTrm != null)
        {
            ActivateRandomChildWithPrefix(visualTrm, "bottom");
            ActivateRandomChildWithPrefix(visualTrm, "hair");
            ActivateRandomChildWithPrefix(visualTrm, "top");
        }
    }

    private void ActivateRandomChildWithPrefix(Transform parentTransform, string prefix)
    {
        List<GameObject> matchingObjects = new List<GameObject>();

        foreach (Transform child in parentTransform)
        {
            if (child.name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                matchingObjects.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }

        if (matchingObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, matchingObjects.Count);
            matchingObjects[randomIndex].SetActive(true);
        }
    }
}
