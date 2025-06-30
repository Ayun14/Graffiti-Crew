using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public enum FightUIAnimationType {
    FightStart,
    Tension
}
public class FightAnimationView : UIView {
    private VisualElement _startAnimation;
    private VisualElement _countDownAnimation;
    private VisualElement _endAnimation;
    private VisualElement _rivalCheckAnimation;
    private VisualElement _sprayWarningAnimation;

    private VisualElement _lineBackground;
    private VisualElement _colorBackground;
    private VisualElement _blueLine;
    private VisualElement _rivalFace;

    private VisualElement _movieTopBorder;
    private VisualElement _movieBottomBorder;

    private VisualElement _warningBackground;
    private Label _warningText;

    public FightAnimationView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        UIAnimationEvent.SetActiveStartAnimationEvnet += SetActiveStartAnimation;
        UIAnimationEvent.SetActiveCountDownAnimationEvnet += SetActiveCountDownAnimation;
        UIAnimationEvent.SetActiveEndAnimationEvnet += SetActiveEndAnimation;
        UIAnimationEvent.SetActiveRivalCheckAnimationEvnet += RivalCheckAnimation;
        UIAnimationEvent.SetPlayerBackgroundColor += SetPlayerBackgroundColor;
        UIAnimationEvent.SetRivalBackgroundColor += SetRivalBackgroundColor;
        UIAnimationEvent.SetFilmDirectingEvent += SetFilmDirecting;
        UIAnimationEvent.SetSprayWarningEvent += SetSprayWatning;
    }

    public override void Dispose() {
        UIAnimationEvent.SetActiveStartAnimationEvnet -= SetActiveStartAnimation;
        UIAnimationEvent.SetActiveCountDownAnimationEvnet -= SetActiveCountDownAnimation;
        UIAnimationEvent.SetActiveEndAnimationEvnet -= SetActiveEndAnimation;
        UIAnimationEvent.SetActiveRivalCheckAnimationEvnet -= RivalCheckAnimation;
        UIAnimationEvent.SetPlayerBackgroundColor -= SetPlayerBackgroundColor;
        UIAnimationEvent.SetFilmDirectingEvent -= SetFilmDirecting;
        UIAnimationEvent.SetSprayWarningEvent -= SetSprayWatning;
        base.Dispose();
    }

    protected override void SetVisualElements() {
        base.SetVisualElements();
        _startAnimation = topElement.Q<VisualElement>("startAnimation");
        _endAnimation = topElement.Q<VisualElement>("endAnimation");
        _rivalCheckAnimation = topElement.Q<VisualElement>("rivalCheckAnimation");
        _countDownAnimation = topElement.Q<VisualElement>("countdownAnimation");
        _sprayWarningAnimation = topElement.Q<VisualElement>("sprayWarningAnimation");

        _lineBackground = _rivalCheckAnimation.Q<VisualElement>("line-background");
        _colorBackground = _rivalCheckAnimation.Q<VisualElement>("color-background");
        _blueLine = _rivalCheckAnimation.Q<VisualElement>("blue-line");
        _rivalFace = _rivalCheckAnimation.Q<VisualElement>("rival-face");

        _movieTopBorder = topElement.Q<VisualElement>("movie-top-border");
        _movieBottomBorder = topElement.Q<VisualElement>("movie-bottom-border");

        _warningBackground = _sprayWarningAnimation.Q<VisualElement>("warning-content");
        _warningText = _sprayWarningAnimation.Q<Label>("warnging-text");
    }

    #region Handles
    private void SetFilmDirecting(bool active) {
        if (active) {
            _movieTopBorder.AddToClassList("movie-top-center");
            _movieBottomBorder.AddToClassList("movie-bottom-center");
        }
        else {
            _movieTopBorder.RemoveFromClassList("movie-top-center");
            _movieBottomBorder.RemoveFromClassList("movie-bottom-center");
        }
    }
    private void SetActiveStartAnimation(bool active) {
        if (active) {
            Show(_startAnimation);
        }
        else {
            Hide(_startAnimation);
        }
    }
    private void SetActiveCountDownAnimation(bool active) {
        if (active) {
            Show(_countDownAnimation);
        }
        else {
            Hide(_countDownAnimation);
        }
    }
    private void SetActiveEndAnimation(bool active) {
        if (active) {
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
    private void SetSprayWatning(bool active) {
        if (active) {
            Show(_sprayWarningAnimation);
            StartSprayWarning();
        }
        else {
            Hide(_sprayWarningAnimation);
        }
    }
    private void SetPlayerBackgroundColor() {
        VisualElement playerScreen = _endAnimation.Q<VisualElement>("player-screen");
        playerScreen.AddToClassList("winner");
    }
    private void SetRivalBackgroundColor() {
        VisualElement rivalcreen = _endAnimation.Q<VisualElement>("rival-screen");
        rivalcreen.AddToClassList("winner");
    }
    #endregion

    private async void StartRivalCheck() {
        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.RivalCheck);

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
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Clock);
    }
    private async void StartSprayWarning() {
        _warningBackground.AddToClassList("warning-show");
        await Task.Delay(100);
        _warningText.AddToClassList("warning-text-show");

        await Task.Delay(3000);

        _warningText.RemoveFromClassList("warning-text-show");
        _warningBackground.RemoveFromClassList("warning-show");
    }

    private void Show(VisualElement view) {
        view.style.display = DisplayStyle.Flex;
    }
    private void Hide(VisualElement view) {
        view.style.display = DisplayStyle.None;
    }
}
