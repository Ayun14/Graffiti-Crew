using UnityEngine;
using AH.SaveSystem;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Computer : InteractionObject
{
    private AudioSource _hangoutBGM;

    protected override void Awake()
    {
        base.Awake();
    }

    private async void Start()
    {
        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        _hangoutBGM = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.HangOut, true);
    }

    public async void ComputerSignal()
    {
        PresentationEvents.FadeInOutEvent?.Invoke(false);
        await Task.Delay(1100);

        _hangoutBGM?.GetComponent<SoundObject>().PushObject();
        SceneManager.LoadScene("ComputerScene");
    }
}
