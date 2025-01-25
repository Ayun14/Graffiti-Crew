using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class IndexElement : BindableElement {
        public int index { get; set; }

        public new class UxmlFactory : UxmlFactory<IndexElement, UxmlTraits> { }
        public new class UxmlTraits : BindableElement.UxmlTraits {
            // 중괄호 열고 public변수는 값을 바로 생성해줄 수 있음
            UxmlIntAttributeDescription m_buttonIndex = new UxmlIntAttributeDescription {
                name = "button-index",
                defaultValue = 0
            };
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) { // 텍스트 파일의 값들을 읽어 올께
                base.Init(ve, bag, cc);

                var dve = ve as IndexElement;

                dve.index = m_buttonIndex.GetValueFromBag(bag, cc);
            }
        }
    }
}
