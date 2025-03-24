using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.CustomElement {
    public class StagePointElement : VisualElement {
        public StageType type { get; set; } = StageType.Stage;
        private bool _canPlay;
        public bool canPlay {
            get => _canPlay;
            set {
                _canPlay = value;
                UpdateStagePointDisplay(_canPlay);
            }
        }
        public string chapter { get; set; }
        public string stage { get; set; }
        private int _starCount;

        private Sprite[] _stars;

        public int starCount {
            get => _starCount;
            set {
                _starCount = value;
                UpdateStarDisplay(); 
            }
        }

        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<StagePointElement, UxmlTraits> { }

        [System.Obsolete]
        public new class UxmlTraits : BindableElement.UxmlTraits {
            UxmlEnumAttributeDescription<StageType> m_type = new UxmlEnumAttributeDescription<StageType> {
                name = "type",
                defaultValue = StageType.Stage 
            };
            UxmlBoolAttributeDescription m_canPlay = new UxmlBoolAttributeDescription {
                name = "canPlay",
                defaultValue = false
            };
            UxmlStringAttributeDescription m_chapter = new UxmlStringAttributeDescription {
                name = "chapter",
                defaultValue = "1"
            };
            UxmlStringAttributeDescription m_stage = new UxmlStringAttributeDescription {
                name = "stage",
                defaultValue = "1"
            };
            UxmlIntAttributeDescription m_starCount = new UxmlIntAttributeDescription {
                name = "starCount",
                defaultValue = 0
            };
            // 스테이지 타입 넣을거면 넣기(의뢰인지, 대결인지)
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) { // �ؽ�Ʈ ������ ������ �о� �ò�
                base.Init(ve, bag, cc);

                var dve = ve as StagePointElement;

                dve.chapter = m_chapter.GetValueFromBag(bag, cc);
                dve.stage = m_stage.GetValueFromBag(bag, cc);
                dve.starCount = m_starCount.GetValueFromBag(bag, cc);
                dve.canPlay = m_canPlay.GetValueFromBag(bag, cc);
                dve.type = m_type.GetValueFromBag(bag, cc);

                VisualElement container = new VisualElement();
                VisualElement element = new VisualElement();
                container.Add(element);
            }
        }

        private VisualElement _starBorder;
        private VisualElement _star1;
        private VisualElement _star2;
        private VisualElement _star3;

        public StagePointElement() {
            _starBorder = new VisualElement() { name = "star-border",
                style = {
                    bottom = Length.Percent(-19),
                    alignItems =Align.Center,
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceAround,
                    width = Length.Percent(100),
                    height = 100
                }
            };
            Add(_starBorder);

            _star1 = new VisualElement() { name = "star1" };
            _star2 = new VisualElement() { name = "star2"};
            _star3 = new VisualElement() { name = "star3"};

            ApplyStarBorderStyle(_star1);
            ApplyStarBorderStyle(_star2);
            ApplyStarBorderStyle(_star3);

            _starBorder.Add(_star1);
            _starBorder.Add(_star2);
            _starBorder.Add(_star3);
        }
        void ApplyStarBorderStyle(VisualElement element) {
            element.style.width = 100;
            element.style.height = 100;
        }
        private void UpdateStarDisplay() {
            _stars = Resources.LoadAll<Sprite>("UI/Stage/Stars");
            StyleBackground star1 = new StyleBackground(_stars[0]);
            StyleBackground star2 = new StyleBackground(_stars[1]);
            StyleBackground star3 = new StyleBackground(_stars[2]);
            _star1.style.backgroundImage = (starCount >= 1) ? star1 : null;
            _star2.style.backgroundImage = (starCount >= 2) ? star2 : null;
            _star3.style.backgroundImage = (starCount >= 3) ? star3 : null;
        }
        private void UpdateStagePointDisplay(bool isClear) {
            if (isClear) {
                this.RemoveFromClassList("not-clear");
            }
            else {
                this.AddToClassList("not-clear");
            }
        }
    }
}