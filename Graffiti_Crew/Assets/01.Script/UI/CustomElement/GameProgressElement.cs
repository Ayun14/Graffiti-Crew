using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.CustomElement {
    public class GameProgressElement : VisualElement {
        public new class UxmlFactory : UxmlFactory<GameProgressElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits {
            UxmlFloatAttributeDescription _min = new UxmlFloatAttributeDescription { name = "min", defaultValue = 0f };
            UxmlFloatAttributeDescription _max = new UxmlFloatAttributeDescription { name = "max", defaultValue = 1f };
            UxmlFloatAttributeDescription _value = new UxmlFloatAttributeDescription { name = "value", defaultValue = 0f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var el = ve as GameProgressElement;
                el.Min = _min.GetValueFromBag(bag, cc);
                el.Max = _max.GetValueFromBag(bag, cc);
                el.Value = _value.GetValueFromBag(bag, cc);
            }
        }

        private VisualElement _progressFill;
        private VisualElement _handle;
        private VisualElement _jiaImg;
        private VisualElement _rivalImg;

        private float min = 0;
        private float max = 1;
        private float value = 0;

        public float Min {
            get => min;
            set { min = value; UpdateProgress(); }
        }
        public float Max {
            get => max;
            set { max = value; UpdateProgress(); }
        }
        public float Value {
            get => value;
            set {
                this.value = Mathf.Clamp(value, min, max);
                UpdateProgress();
            }
        }

        public GameProgressElement() {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.FlexStart;

            AddToClassList("game-progress-element");

            _progressFill = new VisualElement();
            _progressFill.AddToClassList("progress-fill");
            hierarchy.Add(_progressFill);

            _handle = new VisualElement();
            _handle.AddToClassList("progress-handle");
            hierarchy.Add(_handle);

            _jiaImg = new VisualElement();
            _jiaImg.AddToClassList("progress-img");
            _handle.Add(_jiaImg);

            _rivalImg = new VisualElement();
            _rivalImg.AddToClassList("progress-img");
            _handle.Add(_rivalImg);
        }
        public void SetImage(Sprite jia, Sprite rival = null) {
            if (jia) {
                Debug.Log(jia.name);
                _jiaImg.style.backgroundImage = new StyleBackground(jia);
            }
            if (rival) {
                Debug.Log(rival.name);
                _rivalImg.style.backgroundImage = new StyleBackground(rival);
            }
        }
        private void UpdateProgress() {
            float percent = Mathf.InverseLerp(min, max, value);

            // 1. 채운 진행률 설정 (퍼센트 기반)
            _progressFill.style.width = Length.Percent(percent * 100f);

            // 2. 실제 픽셀 계산
            float fillWidth = _progressFill.resolvedStyle.width;
            float handleWidth = _handle.resolvedStyle.width;

            if (fillWidth > 0 && handleWidth > 0) {
                // 3. handle의 중심이 fill의 끝에 오도록 left 설정
                float handleLeft = fillWidth - (handleWidth * 0.5f);
                _handle.style.left = handleLeft;
            }
            else {
                // fallback (렌더링 전이거나 layout 미정일 경우)
                _handle.style.left = Length.Percent(percent * 100f);
            }
        }
    }
}