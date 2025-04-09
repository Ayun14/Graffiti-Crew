using AH.UI.ViewModels;
using AH.UI.Views;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public enum FightUIAnimationType {
    FightStart,
    Tension
}
public class FightAnimationView : UIView {
    private VisualElement _startAnimation;
    private VisualElement _endAnimation;
    private VisualElement _rivalCheckAnimation;

    private VisualElement _lineBackground;
    private VisualElement _colorBackground;
    private VisualElement _blueLine;
    private VisualElement _rivalFace;
    
    public FightAnimationView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        UIAnimationEvent.SetActiveStartAnimationEvnet += SetActiveStartAnimation;
        UIAnimationEvent.SetActiveEndAnimationEvnet += SetActiveEndAnimation;
        UIAnimationEvent.SetActiveRivalCheckAnimationEvnet += RivalCheckAnimation;
    }

    public override void Dispose() { 
        UIAnimationEvent.SetActiveStartAnimationEvnet -= SetActiveStartAnimation;
        UIAnimationEvent.SetActiveEndAnimationEvnet -= SetActiveEndAnimation;
        UIAnimationEvent.SetActiveRivalCheckAnimationEvnet -= RivalCheckAnimation;
        base.Dispose();
    }
    public override void Initialize() {
        base.Initialize();
    }
    protected override void SetVisualElements() {
        base.SetVisualElements();
        _startAnimation = topElement.Q<VisualElement>("startAnimation");
        _endAnimation = topElement.Q<VisualElement>("endAnimation");
        _rivalCheckAnimation = topElement.Q<VisualElement>("rivalCheckAnimation");

        _lineBackground = _rivalCheckAnimation.Q<VisualElement>("line-background");
        _colorBackground = _rivalCheckAnimation.Q<VisualElement>("color-background");
        _blueLine = _rivalCheckAnimation.Q<VisualElement>("blue-line");
        _rivalFace = _rivalCheckAnimation.Q<VisualElement>("rival-face");
    }

    #region Handles
    private void SetActiveStartAnimation(bool active) {
        if (active) {
            Show(_startAnimation);
        }
        else {
            Hide(_startAnimation);
        }
    }
    private void SetActiveEndAnimation(bool active) {
        if (active) {
            //var rivalScreen = _startAnimation.Q<VisualElement>("rival-screen");
            //var playerScreen = _startAnimation.Q<VisualElement>("player-screen");
            //rivalScreen.AddToClassList("move-left");
            //rivalScreen.AddToClassList("move-right");
            Show(_endAnimation);
        }
        else {
            Hide(_endAnimation);
        }
    }
    private void RivalCheckAnimation(bool active) {
        if (active) {
            Show(_rivalCheckAnimation);
            StartRivalCheck();
        }
        else {
            Hide(_rivalCheckAnimation);
        }
    }
    #endregion
    private async void StartRivalCheck() {
        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.RivalCheck);

        // In
        _colorBackground.AddToClassList("default-color-move-screen");
        _rivalFace.AddToClassList("default-face-move-screen");
        await Task.Delay(100);
        _lineBackground.AddToClassList("default-whiteline-move-screen");
        await Task.Delay(100);
        _blueLine.AddToClassList("default-blueline-move-screen");

        // Wait
        await Task.Delay(2000);

        // Out
        _colorBackground.AddToClassList("default-color-move-outside");
        _rivalFace.AddToClassList("default-face-move-outside");
        await Task.Delay(100);
        _lineBackground.AddToClassList("default-whiteline-move-outside");
        await Task.Delay(100);
        _blueLine.AddToClassList("default-blueline-move-outside");

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Clock, true);
    }
    private void Show(VisualElement view) {
        view.style.display = DisplayStyle.Flex;
    }
    private void Hide(VisualElement view) {
        view.style.display = DisplayStyle.None;
    }
}
