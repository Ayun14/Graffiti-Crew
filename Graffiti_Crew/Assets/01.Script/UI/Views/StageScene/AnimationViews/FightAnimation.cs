using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

public enum FightUIAnimationType {
    FightStart,
    Tension
}
public class FightAnimation : UIView {
    private VisualElement _startAnimation;
    private VisualElement _tensionAnimation;
    
    public FightAnimation(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        UIAnimationEvent.SetActiveStartAnimationEvnet += SetActiveStartAnimation;
        UIAnimationEvent.SetActiveTensionAnimationEvnet += TensionAnimation;
    }

    public override void Dispose() { 
        UIAnimationEvent.SetActiveStartAnimationEvnet -= SetActiveStartAnimation;
        UIAnimationEvent.SetActiveTensionAnimationEvnet -= TensionAnimation;
        base.Dispose();
    }
    public override void Initialize() {
        base.Initialize();
    }
    protected override void SetVisualElements() {
        base.SetVisualElements();
        _startAnimation = topElement.Q<VisualElement>("startAnimation");
        Debug.Log(_startAnimation);
        _tensionAnimation = topElement.Q<VisualElement>("tensionAnimation");
        Debug.Log(_tensionAnimation);
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
    private void TensionAnimation(bool active) {
        if (active) {
            Show(_tensionAnimation);
        }
        else {
            Hide(_tensionAnimation);
        }
    } 
    #endregion

    private void Show(VisualElement view) {
        view.style.display = DisplayStyle.Flex;
    }
    private void Hide(VisualElement view) {
        view.style.display = DisplayStyle.None;
    }
}
