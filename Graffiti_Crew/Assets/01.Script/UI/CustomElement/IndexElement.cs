using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class IndexElement : BindableElement {
        public int index { get; set; }

        public new class UxmlFactory : UxmlFactory<IndexElement, UxmlTraits> { }
        public new class UxmlTraits : BindableElement.UxmlTraits {
            // �߰�ȣ ���� public������ ���� �ٷ� �������� �� ����
            UxmlIntAttributeDescription m_buttonIndex = new UxmlIntAttributeDescription {
                name = "button-index",
                defaultValue = 0
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) { // �ؽ�Ʈ ������ ������ �о� �ò�
                base.Init(ve, bag, cc);

                var dve = ve as IndexElement;

                dve.index = m_buttonIndex.GetValueFromBag(bag, cc);
            }
        }
    }
}
