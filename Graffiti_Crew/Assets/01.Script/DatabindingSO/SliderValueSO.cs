using UnityEngine;

[CreateAssetMenu(fileName = "SliderValueSO", menuName = "SO/UI/SliderValueSO")]
public class SliderValueSO : ScriptableObject
{
    public float min = 0;
    public float max = 100;
    public float _value;
    public float Value
    {
        get { return _value; }
        set { _value = Mathf.Clamp(value, min, max); }
    }
}
