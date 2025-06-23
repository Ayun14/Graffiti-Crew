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
        Material randMat = _materialList[Random.Range(0, _materialList.Count)];
        if (1 < _renderer.materials.Length)
        {
            for (int i = 0; i < _renderer.materials.Length; ++i)
                _renderer.materials[i] = randMat;
        }
        else
            _renderer.material = randMat;
    }
}
