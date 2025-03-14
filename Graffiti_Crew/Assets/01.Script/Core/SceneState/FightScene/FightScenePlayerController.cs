using UnityEngine;

public class FightScenePlayerController : Observer<GameStateController>
{
    // 여기에 플레이어 뭐 애니메이션... 위치.. 등등 구현하기
    private Transform _player;
    private Transform _graffitiTrm;
    private Transform _resultTrm;

    private void Awake()
    {
        Attach();

        _player = transform.Find("Player").GetComponent<Transform>();
        _graffitiTrm = transform.Find("GraffitiPos").GetComponent<Transform>();
        _resultTrm = transform.Find("ResultPos").GetComponent<Transform>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.CountDown)
            {
                _player.position = _graffitiTrm.position;
                _player.localRotation = _graffitiTrm.localRotation;
            }

            if (mySubject.GameState == GameState.Finish)
            {
                _player.position = _resultTrm.position;
                _player.localRotation = _resultTrm.localRotation;
            }
        }
    }
}
