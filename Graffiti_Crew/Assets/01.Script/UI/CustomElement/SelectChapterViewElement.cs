using UnityEngine.UIElements;

public enum OtherSceneType {

}
namespace AH.UI.CustomElement
{
    public class SelectChapterViewElement : VisualElement
    {
        public string chapter { get; set; }
        [System.Obsolete]
        public new class UxmlFactory : UxmlFactory<SelectChapterViewElement, UxmlTraits> { }
        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_chapter = new UxmlStringAttributeDescription
            {
                name = "chapter",
                defaultValue = "1"
            };
            // 스테이지 타입 넣을거면 넣기(의뢰인지, 대결인지)
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var dve = ve as SelectChapterViewElement;

                dve.chapter = m_chapter.GetValueFromBag(bag, cc);
            }
        }
    }
}