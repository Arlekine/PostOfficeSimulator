using System;
using UnityEngine;

public class PackageHolder : MonoBehaviour
{
    public Action<PackageHolder> OnPackageIssued;
    
    [SerializeField] private Package _package;
    private DraggableBody _draggableBody;

    public DraggableBody DraggableBody
    {
        get
        {
            if (_draggableBody == null)
                _draggableBody = GetComponent<DraggableBody>();
            
            return _draggableBody;
        }
    }
    public Package Package => _package;

    public void IssuePackage()
    {
        OnPackageIssued?.Invoke(this);
    }
}