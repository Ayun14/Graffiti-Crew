using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Computer : InteractionObject
{
    [SerializeField] private GameObject _transitionCanvas;
    [SerializeField] private Material _transitionMat;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override async void Start()
    {
        base.Start();

        _transitionCanvas.SetActive(false);
        _transitionMat.SetFloat("_Lerp", 1);

        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.HangOut);
    }

    public void ComputerEvent()
    {
        StartCoroutine(ComputerRoutine());
    }

    private IEnumerator ComputerRoutine()
    {
        _transitionCanvas.SetActive(true);
        _transitionMat.DOFloat(0.2f, "_Lerp", 1.5f);
        yield return new WaitForSeconds(2f);

        _transitionMat.DOFloat(0f, "_Lerp", 0.3f);
        yield return new WaitForSeconds(0.8f);

        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.HangOut);
        SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
    }
}
