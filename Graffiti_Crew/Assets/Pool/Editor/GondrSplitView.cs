using UnityEngine.UIElements;

public class GondrSplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<GondrSplitView, UxmlTraits>{}
    public new class UxmlTraits : TwoPaneSplitView.UxmlTraits {}
}
