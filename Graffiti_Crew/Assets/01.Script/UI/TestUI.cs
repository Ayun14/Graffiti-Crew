using AH.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUI : UIManagement {
    private VisualElement checker;
    private VisualElement test;

    protected override void SetupViews() {
        base.SetupViews();
        VisualElement root = _uiDocument.rootVisualElement;
        checker = root.Q<VisualElement>("buy-btn");
    }
    protected override void Register() {
        base.Register();
        checker.RegisterCallback<PointerDownEvent>(evt => {
            Debug.Log("pointer down");
        });
    }
}
