using UnityEngine.UIElements;

namespace AH.UI.CustomElement {
    public class StagePointElement : VisualElement {
        public string chapter { get; set; }
        public string stage { get; set; }

        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<StagePointElement, UxmlTraits> { }

        [System.Obsolete]
        public new class UxmlTraits : BindableElement.UxmlTraits {
            UxmlStringAttributeDescription m_chapter = new UxmlStringAttributeDescription {
                name = "chapter",
                defaultValue = "1"
            };
            UxmlStringAttributeDescription m_stage = new UxmlStringAttributeDescription {
                name = "stage",
                defaultValue = "1"
            };
            // 스테이지 타입 넣을거면 넣기(의뢰인지, 대결인지)
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) { // �ؽ�Ʈ ������ ������ �о� �ò�
                base.Init(ve, bag, cc);

                var dve = ve as StagePointElement;

                dve.chapter = m_chapter.GetValueFromBag(bag, cc);
                dve.stage = m_stage.GetValueFromBag(bag, cc);
            }
        }
    }
}