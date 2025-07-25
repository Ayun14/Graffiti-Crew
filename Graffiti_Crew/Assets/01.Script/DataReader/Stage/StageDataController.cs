using AH.UI.Events;
using UnityEngine;

public class StageDataController : MonoBehaviour
{
    [SerializeField] private StageDataReader _stageData;
    [SerializeField] private StageDescriptionSO _stageDescriptionSO;
    [SerializeField] private TMIDataReader _sprayData;

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
        for (int i = 0; i < _stageData.StageList.Count; i++)
        {
            if(_stageData.StageList[i].id == chapter+stage)
            {
                _stageDescriptionSO.title = _stageData.StageList[i].title;
                _stageDescriptionSO.description = _stageData.StageList[i].description;
                _stageDescriptionSO.graffiti = Resources.Load<Sprite>($"UI/Stage/Sprite/{_stageData.StageList[i].sprite}");
                //_stageDescriptionSO.ticket = _sprayData.ConversionSprayData(i);
                break;
            }
        }
    }
}
