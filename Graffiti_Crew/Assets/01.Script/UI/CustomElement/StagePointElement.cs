﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.CustomElement {
    public class StagePointElement : VisualElement {
        private StageType _stageType;
        public StageType StageType {
            get => _stageType;
            set {
                _stageType = value;
                SetName();
            }
        }

        private StageState _stageState;
        public StageState StageState {
            get => _stageState;
            set {
                _stageState = value;
                ChangeSetImg();
            }
        }

        private string _chapter { get; set; }
        public string chapter {
            get => _chapter;
            set {
                _chapter = value;
                SetName();
            }
        }

        private string _stage { get; set; }
        public string stage {
            get => _stage;
            set {
                _stage = value;
                SetName();
            }
        }

        private string _imageNumber;
        public string imageNumber {
            get => _imageNumber;
            set {
                _imageNumber = value;
            }
        }

        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<StagePointElement, UxmlTraits> { }

        [System.Obsolete]
        public new class UxmlTraits : BindableElement.UxmlTraits {
            UxmlEnumAttributeDescription<StageType> m_stageType = new UxmlEnumAttributeDescription<StageType> {
                name = "stageType",
                defaultValue = StageType.Battle
            };
            UxmlEnumAttributeDescription<StageState> m_stageState = new UxmlEnumAttributeDescription<StageState> {
                name = "stageState",
                defaultValue = StageState.Lock
            };
            UxmlStringAttributeDescription m_chapter = new UxmlStringAttributeDescription {
                name = "chapter",
                defaultValue = "1"
            };
            UxmlStringAttributeDescription m_stage = new UxmlStringAttributeDescription {
                name = "stage",
                defaultValue = "1"
            };
            UxmlStringAttributeDescription m_imageNumber = new UxmlStringAttributeDescription {
                name = "imageNumber",
                defaultValue = "1"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) { // �ؽ�Ʈ ������ ������ �о� �ò�
                base.Init(ve, bag, cc);

                var dve = ve as StagePointElement;

                dve.chapter = m_chapter.GetValueFromBag(bag, cc);
                dve.stage = m_stage.GetValueFromBag(bag, cc);
                dve.StageState = m_stageState.GetValueFromBag(bag, cc);
                dve.StageType = m_stageType.GetValueFromBag(bag, cc);
                dve._imageNumber = m_imageNumber.GetValueFromBag(bag, cc);

                dve.Setting();
                dve.SetName();
            }
        }

        private VisualElement _canPlay;
        private VisualElement _lock;
        public StagePointElement() {
            _canPlay = new VisualElement() {
                name = "can-play",
                style = {
                    position = Position.Absolute,
                    width = Length.Percent(100),
                    height = Length.Percent(100)
                }
            };
            _lock = new VisualElement() {
                name = "lock",
                style = {
                    position = Position.Absolute,
                    width = Length.Percent(100),
                    height = Length.Percent(100)
                }
            };

            this.Add(_canPlay);
            this.Add(_lock);
        }
        private void Setting() {
            string lockImgPath = $"UI/Stage/Map/Chapter{_chapter}/{_chapter}-{_imageNumber}-lock";
            string canplayImgPath = $"UI/Stage/Map/Chapter{_chapter}/{_chapter}-{_imageNumber}-canplay";

            Sprite lockImg = Resources.Load<Sprite>(lockImgPath);
            Sprite canplayImg = Resources.Load<Sprite>(canplayImgPath);

            if(lockImg==null && canplayImg== null) {
                return;
            }

            _lock.style.backgroundImage = new StyleBackground(lockImg);
            _canPlay.style.backgroundImage = new StyleBackground(canplayImg);

            float imgWidth = lockImg.rect.width;
            float imgHeight = lockImg.rect.height;

            float percentX = (imgWidth / 1920f) * 100f;
            float roundedPercentX = (float)Math.Round(percentX, 2); 

            float percentY = (imgHeight / 1080f) * 100f;
            float roundedPercentY = (float)Math.Round(percentY, 2);

            _lock.parent.style.width = Length.Percent(roundedPercentX);
            _lock.parent.style.height = Length.Percent(roundedPercentY);
        }
        private void SetName() {
            name = $"Chapter{_chapter}-{_stageType}{_stage}";
        }
        private void ChangeSetImg() {
            switch (_stageState) {
                case StageState.Clear:
                    _canPlay.style.display = DisplayStyle.None;
                    _lock.style.display = DisplayStyle.None;
                    break;
                case StageState.CanPlay:
                    _canPlay.style.display = DisplayStyle.Flex;
                    _lock.style.display = DisplayStyle.None;
                    break;
                case StageState.Lock:
                    _canPlay.style.display = DisplayStyle.Flex;
                    _lock.style.display = DisplayStyle.Flex;
                    break;
            }
        }
    }
}