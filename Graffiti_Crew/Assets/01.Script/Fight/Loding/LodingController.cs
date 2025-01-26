using AH.Map;
using UnityEngine;

public class LodingController : MonoBehaviour
{
    [SerializeField] private LoadStageSO _stageSO;

    private StageDataSO _stageData;

    private void Awake()
    {
        FindStageData();
        GiveData();
    }

    private void FindStageData()
    {
        _stageData = Resources.Load(_stageSO.GetLoadStageName()) as StageDataSO;
    }

    private void GiveData()
    {
        if (_stageData != null)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("NeedLoding");

            foreach (GameObject obj in objects)
            {
                if (obj.TryGetComponent(out INeedLoding needLoding))
                    needLoding.LodingHandle(_stageData);
            }
        }
    }
}
