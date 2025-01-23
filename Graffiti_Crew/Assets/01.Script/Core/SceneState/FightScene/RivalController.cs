using UnityEngine;

public class RivalController : Observer<GameStateController>
{
    // 라이벌을 SO로 관리하는게 좋을듯
    // 그릴 그래피티 및 그리는 시간이 담긴 list, 이름, 버프내용, 대사 등등...

    [SerializeField] private Sprite _graffiti;
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
}
