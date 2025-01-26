using AH.Map;
using UnityEngine;

public class LodingController : Observer<GameStateController>
{
    [SerializeField] private LoadStageSO _stageSO;

    private StageDataSO _stageData;

    private void Awake()
    {
        Attach();


    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject!= null)
        {

        }
    }
}
