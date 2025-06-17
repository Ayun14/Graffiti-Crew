using UnityEngine.UIElements;

public enum OtherSceneType {
    TitleScene,
    HangOutScene,
    ComputerScene
}
namespace AH.UI.CustomElement
{
    public class SettingViewElement : VisualElement
    {
        private OtherSceneType _sceneType;
        public OtherSceneType SceneType {
            get => _sceneType;
            set {
                _sceneType = value;
            }
        }
        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<SettingViewElement, UxmlTraits> { }
        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlEnumAttributeDescription<OtherSceneType> m_sceneType = new UxmlEnumAttributeDescription<OtherSceneType> {
                name = "sceneType",
                defaultValue = OtherSceneType.HangOutScene
            };
            // 스테이지 타입 넣을거면 넣기(의뢰인지, 대결인지)
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var dve = ve as SettingViewElement;
                dve.SceneType = m_sceneType.GetValueFromBag(bag, cc);
            }
        }
    }
}