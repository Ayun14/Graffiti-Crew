using AH.SaveSystem;
using System;
using UnityEngine;

public class NPCChecker : MonoBehaviour
{
    [SerializeField] private StageSaveDataSO _lastStageDataSO;

    private void Start()
    {
        CheckStageClear();
    }

    private void CheckStageClear()
    {

    }
}
