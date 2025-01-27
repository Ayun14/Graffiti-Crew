using UnityEngine;

public class RivalController : Observer<GameStateController>, INeedLoding
{
    private Sprite _graffiti;
    private SpriteRenderer _graffitiRenderer;

    private void Awake()
    {
        Attach();

        _graffitiRenderer = GetComponentInChildren<SpriteRenderer>();
        _graffitiRenderer.sprite = null;
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
                SetGraffiti(null);
        }
    }

    private void SetGraffiti(Sprite sprite)
    {
        _graffitiRenderer.sprite = _graffiti;
    }

    public void LodingHandle(StageDataSO stageData)
    {
        _graffiti = stageData.rivalGraffiti;
    }
}
