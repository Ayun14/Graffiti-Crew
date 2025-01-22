using UnityEngine;

public class RivalController : Observer<GameStateController>
{
    // ���̹��� SO�� �����ϴ°� ������
    // �׸� �׷���Ƽ �� �׸��� �ð��� ��� list, �̸�, ��������, ��� ���...

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
