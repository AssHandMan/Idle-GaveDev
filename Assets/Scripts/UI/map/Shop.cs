using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] public bool _isActive = false;
    [SerializeField] private GameObject _btn;
    [SerializeField] private Material _material_Floor;
    [SerializeField] private Material _material_Walls;
    [SerializeField] private MeshRenderer _meshRenderer_Floor;
    [SerializeField] private MeshRenderer _meshRenderer_Walls;

    public void Activated()
    {
        _isActive = true;
        _meshRenderer_Floor.material = _material_Floor;
        _meshRenderer_Walls.material = _material_Walls;
        _btn.SetActive(true);

    }
}
