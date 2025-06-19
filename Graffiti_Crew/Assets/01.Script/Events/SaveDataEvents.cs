using System;
using UnityEngine;

public static class SaveDataEvents
{
    public static Action LoadEndEvent;
    public static Action<string> SaveGameEvent;
    public static Action ChangeSlotEvent;
    public static Action DeleteSaveDataEvent;
}
