using UnityEngine;

public class PoliceLight : MonoBehaviour
{
    [SerializeField] private Color _startColor = Color.red;
    [SerializeField] private Color _changeColor = Color.blue;
    [SerializeField] private float _interval = 0.5f;

    private Light _light;
    private float _timer;
    private bool _isStartColor = true;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _light.color = _startColor;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0f;
            ToggleColor();
        }
    }

    private void ToggleColor()
    {
        if (_isStartColor)
            _light.color = _changeColor;
        else
            _light.color = _startColor;

        _isStartColor = !_isStartColor;
    }
}
