using TMPro;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public GameObject speechBubblePrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private Camera _targetCam;
    private GameObject _currentSpeechBubble;

    private void Start()
    {
        _targetCam = Camera.main;
    }

    public void ShowSpeechBubble(Transform targetTransform)
    {
        if (_currentSpeechBubble == null)
        {
            _currentSpeechBubble = Instantiate(speechBubblePrefab, transform);
        }

        _currentSpeechBubble.transform.position = targetTransform.position + offset;
        _currentSpeechBubble.SetActive(true);

        if (_targetCam != null && _currentSpeechBubble.activeSelf)
        {
            _currentSpeechBubble.transform.LookAt(_targetCam.transform);
            _currentSpeechBubble.transform.Rotate(0, 180, 0);
        }
    }

    public void HideSpeechBubble()
    {
        if (_currentSpeechBubble != null)
        {
            _currentSpeechBubble.SetActive(false);
        }
    }

    private void Update()
    {
        if (_currentSpeechBubble != null && _currentSpeechBubble.activeSelf && _targetCam != null)
        {
            _currentSpeechBubble.transform.LookAt(_targetCam.transform);
            _currentSpeechBubble.transform.Rotate(0, 180, 0);
        }
    }
}
