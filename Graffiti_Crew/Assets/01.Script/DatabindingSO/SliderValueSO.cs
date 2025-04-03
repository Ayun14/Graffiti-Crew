using AH.UI.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "SliderValueSO", menuName = "SO/UI/SliderValueSO")]
public class SliderValueSO : ScriptableObject
{
    public float min = 0;
    public float max = 100;
    public float value;

    public float Value
    {
        get { return value; }
        set { 
            this.value = Mathf.Clamp(value, min, max);
        }
    }
}
