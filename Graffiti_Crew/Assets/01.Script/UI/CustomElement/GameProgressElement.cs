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
        private VisualElement _middleLine;

        private bool _layoutReady = false;

        private float min = 0;
        private float max = 1;
        private float value = 0;

        public float Min {
            get => min;
            set {
                if (Mathf.Approximately(min, value)) return;
                min = value;
                UpdateProgress();
            }
        }
        public float Max {
            get => max;
            set {
                if (Mathf.Approximately(max, value)) return;
                max = value;
                UpdateProgress();
            }
        }
        public float Value {
            get => value;
            set {
                float clamped = Mathf.Clamp(value, min, max);
                if (Mathf.Approximately(clamped, this.value)) return;

                this.value = clamped;
                UpdateProgress();
            }
        }

        public GameProgressElement() {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            style.justifyContent = Justify.FlexStart;

            AddToClassList("game-progress-element");

            _progressFill = new VisualElement();
            _progressFill.name = "fill";
            _progressFill.AddToClassList("progress-fill");
            hierarchy.Add(_progressFill);

            _middleLine = new VisualElement();
            _middleLine.name = "middle-line";
            _middleLine.AddToClassList("middle-line");
            hierarchy.Add(_middleLine);

            _handle = new VisualElement();
            _handle.name = "handle";
            _handle.AddToClassList("progress-handle");
            hierarchy.Add(_handle);

            _jiaImg = new VisualElement();
            _jiaImg.name = "jia";
            _jiaImg.AddToClassList("progress-img");
            _handle.Add(_jiaImg);

            _rivalImg = new VisualElement();
            _rivalImg.name = "rival";
            _rivalImg.AddToClassList("progress-img");
            _handle.Add(_rivalImg);


            RegisterCallback<GeometryChangedEvent>(evt => {
                if (!_layoutReady) {
                    _layoutReady = true;
                    UpdateProgress(); // layout 완료 후 정확한 위치 계산
                }
            });
        }
 
        public void SetImage(Sprite jia, Sprite rival = null) {
            if (jia) {
                _jiaImg.style.backgroundImage = new StyleBackground(jia);
            }
            if (rival) {
                _rivalImg.style.backgroundImage = new StyleBackground(rival);
                _middleLine.style.display = DisplayStyle.Flex;
            }
            else {
                _middleLine.style.display = DisplayStyle.None;
                _rivalImg.style.display = DisplayStyle.None;
            }
        }
        private void UpdateProgress() {
            float percent = Mathf.InverseLerp(min, max, value);
            float clampedPercent = Mathf.Clamp01(percent);
            _progressFill.style.width = Length.Percent(clampedPercent * 100f);

            // handle 배치는 resolvedStyle이 보장될 때만
            schedule.Execute(() => {
                float handleWidth = _handle.resolvedStyle.width;
                float parentWidth = resolvedStyle.width;

                if (handleWidth <= 0 || parentWidth <= 0) {
                    Debug.LogWarning("Delayed layout. Skipping handle position.");
                    return;
                }

                float handleLeft = clampedPercent * parentWidth - (handleWidth * 0.5f);
                handleLeft = Mathf.Clamp(handleLeft, 0f - handleWidth * 0.5f, parentWidth - handleWidth * 0.5f);

                _handle.style.left = handleLeft;
            }).ExecuteLater(0);
        }
    }
}