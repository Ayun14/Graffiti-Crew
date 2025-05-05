using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class AudienceNPCController : Observer<GameStateController>
{
    [SerializeField] private GameObject _maleNCPPrefab;
    [SerializeField] private GameObject _femaleNCPPrefab;
    [Range(0, 11)][SerializeField] private int _minSpawnNum;
    [Range(0, 11)][SerializeField] private int _maxSpawnNum;

    private List<Transform> _spawnPos = new();

    private void Awake()
    {
        Attach();

        _spawnPos = transform.Find("SpawnPos").GetComponentsInChildren<Transform>().ToList();
        _spawnPos.RemoveAt(0);
        AudienceNPCSpawn();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.People_Idle);
                AnimationEvent.SetAnimation?.Invoke(11, AnimationEnum.People_Idle);
                AnimationEvent.SetAnimation?.Invoke(12, AnimationEnum.People_Idle);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.People_Clap);
                AnimationEvent.SetAnimation?.Invoke(11, AnimationEnum.People_Clap);
                AnimationEvent.SetAnimation?.Invoke(12, AnimationEnum.People_Clap);
            }
        }
    }

    private void AudienceNPCSpawn()
    {
        int spawnNum = Random.Range(_minSpawnNum, _maxSpawnNum + 1);
        for (int i = 0; i < spawnNum; ++i)
        {
            GameObject spawnGO = Random.Range(0, 2) == 0 ? _maleNCPPrefab : _femaleNCPPrefab;
            Spawn(spawnGO, _spawnPos[i]);
        }
    }

    private void Spawn(GameObject prefab, Transform spawnTrm)
    {
        GameObject go = Instantiate(prefab, spawnTrm);
        AnimationController animationController = go.GetComponentInChildren<AnimationController>();
        animationController.ObjectID = Random.Range(10, 13);

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
            int randomIndex = UnityEngine.Random.Range(0, matchingObjects.Count);
            matchingObjects[randomIndex].SetActive(true);
        }
    }
}
