using UnityEngine;

[CreateAssetMenu(fileName = "SliderValueSO", menuName = "SO/UI/SliderValueSO")]
public class SliderValueSO : ScriptableObject
{
    public float min = 0;
    public float max = 100;
    public float value;
}
