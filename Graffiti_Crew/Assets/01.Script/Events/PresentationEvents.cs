using System;
using UnityEngine;

public static class PresentationEvents {
    public static Action<bool> FadeInOutEvent;  // true : in | false : out
    public static Action<bool> SetFadeEvent;  // true : in | false : out
}
