using AH.UI.Events;
using NUnit.Framework;
using System;
using UnityEngine;

public class StageDataController : MonoBehaviour
{
    [SerializeField] private StageDataReader _stageData;
    [SerializeField] private StageDescriptionSO _stageDescriptionSO;
    [SerializeField] private SprayDataReader _sprayData;

    private void Start()
    {
        ComputerEvent.SelectStageEvent += HandleLoadStageData;
    }

    private void OnDestroy()
    {
        ComputerEvent.SelectStageEvent -= HandleLoadStageData;
    }

    private void HandleLoadStageData(string chapter, string stage)
    {
        Debug.Log(chapter + stage);
        for (int i = 0; i < _stageData.StageList.Count; i++)
        {
            if(_stageData.StageList[i].id == chapter+stage)
            {
                _stageDescriptionSO.title = _stageData.StageList[i].title;
                _stageDescriptionSO.description = _stageData.StageList[i].description;
                _stageDescriptionSO.ticket = _sprayData.ConversionSprayData();
                break;
            }
        }
    }
}
