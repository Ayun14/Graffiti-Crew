using System.Collections.Generic;
using UnityEngine;

public class AudienceNPCMaterialController : MonoBehaviour
{
    [SerializeField] private List<Material> _materialList = new();

    private SkinnedMeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
        SetMaterial();
    }

    private void SetMaterial()
    {
        _renderer.material = _materialList[Random.Range(0, _materialList.Count)];
    }
}
