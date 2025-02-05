using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SprayBoxController : Observer<GameStateController>
{
    [SerializeField] private float _changeTime = 2f;
    private float _currentTime = 0;
    private bool _isChangingSpray = false;

    // Slider
    private Slider _sprayChangeTimeSlider;

    private void Awake()
    {
        Attach();

        _sprayChangeTimeSlider = transform.Find("Canvas")
            .transform.Find("Slider_SprayChangeTime").GetComponent<Slider>();
        _sprayChangeTimeSlider.value = 0;
        Color color = _sprayChangeTimeSlider.image.color;
        color.a = 0;
        _sprayChangeTimeSlider.image.color = color;

        _currentTime = 0;
    }

    private void Update()
    {
        SprayChangeCheck();
    }

    private void OnDestroy()
    {
        Detach();
    }

    private void SprayChangeCheck()
    {
        if (mySubject.IsSprayEmpty)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                GameObject hitObject = hit.collider.gameObject.transform.parent.gameObject;
                if (hitObject == gameObject && Input.GetMouseButton(0))
                {
                    if (!_isChangingSpray) // ó�� �����Ǿ��� �� �� ���� ����
                    {
                        _sprayChangeTimeSlider.image.DOFade(1, 0.3f);
                        _isChangingSpray = true;
                    }

                    _currentTime += Time.deltaTime;
                    _sprayChangeTimeSlider.value = _currentTime / _changeTime;
                    _sprayChangeTimeSlider.gameObject.SetActive(true);

                    if (_currentTime >= _changeTime)
                    {
                        mySubject.SetIsSprayEmpty(false);
                        mySubject.InvokeSprayChangeEvent();
                        ResetSprayChange();
                    }
                }
                else
                {
                    ResetSprayChange();
                }
            }
        }
    }

    private void ResetSprayChange()
    {
        _sprayChangeTimeSlider.image.DOFade(0, 0.3f);
        _sprayChangeTimeSlider.value = 0;
        _currentTime = 0;
        _isChangingSpray = false;
    }

    public override void NotifyHandle()
    {

    }
}
