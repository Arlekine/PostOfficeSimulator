using UnityEngine;

[CreateAssetMenu(menuName = "DATA/Package", fileName = "Pack_")]
public class Package : ScriptableObject
{
    [SerializeField] private Sprite _packageIcon;

    public Sprite PackageIcon => _packageIcon;
}