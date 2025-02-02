using UnityEngine;

public class SprayBoxController : Observer<GameStateController>
{
    [SerializeField] private float _changeTime = 2f;
    private float _currentTime = 0;

    private void Awake()
    {
        Attach();

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
                if (hitObject == gameObject)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime >= _changeTime)
                    {
                        mySubject.SetIsSprayEmpty(false);
                        mySubject.InvokeSprayChangeEvent();
                    }
                }
                else
                    _currentTime = 0;
            }
        }
    }

    public override void NotifyHandle()
    {

    }
}
