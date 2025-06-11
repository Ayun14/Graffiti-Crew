using TMPro;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public GameObject speechBubblePrefab;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    private GameObject _currentSpeechBubble;

    public void ShowSpeechBubble(Transform targetTransform)
    {
        if (_currentSpeechBubble == null)
        {
            _currentSpeechBubble = Instantiate(speechBubblePrefab, transform);
        }

        _currentSpeechBubble.transform.position = targetTransform.position + offset;
        _currentSpeechBubble.SetActive(true);
    }

    public void HideSpeechBubble()
    {
        if (_currentSpeechBubble != null)
        {
            _currentSpeechBubble.SetActive(false);
        }
    }
}
