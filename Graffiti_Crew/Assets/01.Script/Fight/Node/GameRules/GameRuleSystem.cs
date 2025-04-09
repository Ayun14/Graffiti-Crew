using Unity.VisualScripting;
using UnityEngine;

public class GameRuleSystem : MonoBehaviour, INeedLoding
{
    public void LodingHandle(DataController dataController)
    {
        StageGameRule stageGameRule = null;
        switch (dataController.stageData.stageRuleType)
        {
            case StageRuleType.SpeedRule:
                stageGameRule = transform.AddComponent<SpeedGameRule>();
                break;
            case StageRuleType.PerfectRule:
                stageGameRule = transform.AddComponent<PerfectGameRule>();
                break;
            case StageRuleType.OneTouchRule:
                stageGameRule = transform.AddComponent<OneTouchGameRule>();
                break;
        }
        stageGameRule?.Loding(dataController);
        dataController.SuccessGiveData();
    }
}
