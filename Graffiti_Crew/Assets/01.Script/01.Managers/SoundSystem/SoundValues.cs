using UnityEngine;

public enum SoundType
{
    [Header("SFX")]
    //FightScene
    Spray_Short,
    Spray_Long,
    Spray_NoneGas,
    Spray_Change,
    Spray_Miss,
    Spray_Shake,
    Throw_Egg,
    Win,
    Lose,
    RivalCheck,
    Clock,
    Drum_Kick,
    Drum_Snap,
    DJ_Sound,
    DJ_One,
    DJ_Two,
    DJ_Yeah,
    Coin,
    Drum_Roll,

    // HangOut
    Sofa,
    Typing,
    Walk,

    // UI
    Text_Typing,
    Laughing,
    Paper,
    Sigh,
    Surprised,
    Click_Mouse,
    Click_UI,
    Buy,
    Star,

    // Dialogue
    OpenDoor,
    Wow,
    Shock,
    Knock,
    Running,
    Sniff,
    Twinkle,
    Rain,
    Call,
    EndCall,
    Find,

    [Header("BGM")]
    Title,
    Request,
    Fight_Before,
    Fight_Middle,
    Fight_After,
    Fight_Result,
    HangOut,
    Police,
    Tutorial,
    Serious,
    Serious2,
    Serious3,
    GuitarBGM,
}
