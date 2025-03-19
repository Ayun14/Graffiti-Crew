using UnityEngine;

public class FightScenePlayerController : Observer<GameStateController>
{
    // 여기에 플레이어 뭐 애니메이션... 위치.. 등등 구현하기
    private Transform _player;
    private Transform _resultTrm;

    private void Awake()
    {
        Attach();

        _player = transform.Find("PlayerAnim").GetComponent<Transform>();
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
            if (mySubject.GameState == GameState.Timeline)
            {
                AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Ready);

                _player.position = _resultTrm.position;
                _player.localRotation = _resultTrm.localRotation;
            }
        }
    }
}
