using AH.UI.ViewModels;
using AH.UI.Views;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class FightStartAnimation : UIView {
    private VisualElement _rivalScreen;
    private VisualElement _playerScreen;

    private VisualElement _textContent;

    private int waitTime = 2000;
    public FightStartAnimation(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        UIAnimationEvent.StartFightStartAnimationEvnet += PlayAnimation;
    }

    public override void Dispose() {
        UIAnimationEvent.StartFightStartAnimationEvnet -= PlayAnimation;
        base.Dispose();
    }

    public override void Initialize() {
        base.Initialize();
    }
    protected override void SetVisualElements() {
        base.SetVisualElements();

        _rivalScreen = topElement.Q<VisualElement>("rival-screen");
        _playerScreen = topElement.Q<VisualElement>("player-screen");

        _textContent = topElement.Q<VisualElement>("start-content");
    }
    private async void PlayAnimation() {
        await Task.Delay(waitTime);
        _rivalScreen.AddToClassList("move-left");
        _playerScreen.AddToClassList("move-right");
        await Task.Delay(450);
        _textContent.AddToClassList("size-up");
    }
}
