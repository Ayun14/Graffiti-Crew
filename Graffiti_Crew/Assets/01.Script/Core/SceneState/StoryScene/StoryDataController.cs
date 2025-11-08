using AH.Save;
using System.Threading.Tasks;
using UnityEngine;

public class StoryDataController : DataController
{
    [SerializeField] private StringSaveDataSO _gameProgressSO;

    protected override void NotifyHandleChild()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.NextStage)
            {
                SaveGameProgress();

                GoNextStage();
            }

            if (mySubject.GameState == GameState.Result)
            {
            }
        }
    }

    private void SaveGameProgress()
    {
        string chapter = stageSO.chapter;
        string chapterNumberStr = chapter.Replace("Chapter", "");
        int chapterNumber = int.Parse(chapterNumberStr);

        string stageNumber = stageSO.GetStageNumber(); // ¿¹: "Chapter1Battle1"

        string progress = $"{chapterNumber}-{stageNumber}";
        _gameProgressSO.data = progress;
    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected async override void FinishGiveData()
    {
        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        mySubject.ChangeGameState(GameState.Dialogue);
    }
}
