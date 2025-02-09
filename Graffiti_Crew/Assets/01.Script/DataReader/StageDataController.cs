using AH.UI.Events;
using System;
using UnityEngine;

public class StageDataController : MonoBehaviour
{
    [SerializeField] private StageDataReader _stageData;
    [SerializeField] private StageDescriptionSO _stageDescriptionSO;

    private void Start()
    {
        ComputerEvent.SelectStageEvent += HandleLoadStageData;
    }

    private void OnDisable()
    {
        ComputerEvent.SelectStageEvent -= HandleLoadStageData;
    }

    private void HandleLoadStageData(string chapter, string stage)
    {
        for (int i = 0; i < _stageData.StageList.Count; i++)
        {
            if(_stageData.StageList[i].title == $"{chapter}-{stage}")
            {
                _stageDescriptionSO.title = _stageData.StageList[i].title;
                _stageDescriptionSO.description = _stageData.StageList[i].description;
                break;
            }
        }
    }
}
