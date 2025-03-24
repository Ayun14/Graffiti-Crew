using UnityEngine;

public enum SoundType
{
    [Header("SFX")]
    //FightScene
    Spray_Short,
    Spray_Long,
    Spray_Miss,
    Spray_Shake,
    Throw_Egg,
    Win,
    Lose,
    RivalCheck,
    Clock,
    CountDown,

    // HangOut
    Sofa,
    Typing,
    Walk,

    // UI
    Text_Typing,
    Click_Mouse,
    Click_UI,
    Buy,
    Star,

    [Header("BGM")]
    Title_Front,
    Title_Back,
    Request,
    Fight_Before,
    Fight_Middle,
    Fight_After,
    HangOut,
}
