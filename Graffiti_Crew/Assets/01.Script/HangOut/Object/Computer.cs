using UnityEngine;
using AH.SaveSystem;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class Computer : InteractionObject
{
    [SerializeField] private Material _transitionMat;
    private AudioSource _hangoutBGM;

    protected override void Awake()
    {
        base.Awake();
    }

    private async void Start()
    {
        _transitionMat.SetFloat("_Lerp", 1);

        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        _hangoutBGM = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.HangOut, true);
    }

    public void ComputerEvent()
    {
        StartCoroutine(ComputerRoutine());
    }

    private IEnumerator ComputerRoutine()
    {
        _transitionMat.DOFloat(0.2f, "_Lerp", 1.5f);
        yield return new WaitForSeconds(2.5f);

        _transitionMat.DOFloat(0f, "_Lerp", 0.5f);
        yield return new WaitForSeconds(1f);

        _hangoutBGM?.GetComponent<SoundObject>().PushObject(true);
        SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
    }
}
