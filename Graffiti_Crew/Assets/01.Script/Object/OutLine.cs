using UnityEngine;
using UnityEngine.Rendering;

public class OutLine : MonoBehaviour
{
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private float _outlineScaleFactor = 1.1f;
    [SerializeField] private Color _outlineColor = Color.white;

    private Renderer _outlineRenderer;
    private GameObject _outlineObject;
    private Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (CheckMousePos())
        {
            if (_outlineRenderer == null)
            {
                _outlineRenderer = CreateOutline(_outlineMaterial, _outlineScaleFactor, _outlineColor);
                _outlineRenderer.enabled = true;
            }
        }
        else
        {
            if (_outlineRenderer != null)
            {
                Destroy(_outlineObject);
                _outlineRenderer = null;
            }
        }
    }

    private bool CheckMousePos()
    {
        if (_col != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return _col.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
        }

        return false;
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        _outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = _outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = ShadowCastingMode.Off;

        _outlineObject.GetComponent<OutLine>().enabled = false;
        _outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }
}
