using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Touch;
using UnityEngine;

public class PackehesSelector : MonoBehaviour
{
    public LayerMask trashLayer;
    public LayerMask pointLayer;
    public Camera raycastCamera;

    [Header("Package returning")]
    [SerializeField] private Transform _returningBorder;
    [SerializeField] private Transform _returningPoint;

    private PackageHolder _selectedTrash;

    private void Awake()
    {
        LeanTouch.OnFingerDown += TakeCloth;
        LeanTouch.OnFingerUp += DropCloth;
    }

    private List<PackageHolder> _packages = new List<PackageHolder>();

    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= TakeCloth;
        LeanTouch.OnFingerUp -= DropCloth;
    }

    private void DropCloth(LeanFinger finger)
    {
        if (_selectedTrash != null)
        {
            RaycastHit hit;
            Ray ray = raycastCamera.ScreenPointToRay(finger.ScreenPosition);

            if (Physics.Raycast(ray, out hit, 1000f, layerMask: pointLayer))
            {
                var packagesGettingPoint = hit.collider.GetComponent<PackagesGettingPoint>();
                if (packagesGettingPoint != null && packagesGettingPoint.IsActive)
                {
                    packagesGettingPoint.GivePackage(_selectedTrash);
                }
                else
                {
                    FreeCurrentPackage();
                }
            }
            else
                FreeCurrentPackage();

            _selectedTrash = null;
        }
    }

    private void FreeCurrentPackage()
    {
        if (_selectedTrash == null)
            return;

        if (_selectedTrash.transform.position.z > _returningBorder.position.z)
            _selectedTrash.DraggableBody.MoveToPoint(_returningPoint, 1f, freeAfterPath: true);
        else
            _selectedTrash.DraggableBody.Free();
    }

    private void TakeCloth(LeanFinger finger)
    {
        RaycastHit hit;
        Ray ray = raycastCamera.ScreenPointToRay(finger.ScreenPosition);

        if (Physics.Raycast(ray, out hit, 1000f, layerMask: trashLayer))
        {
            var body = hit.collider.GetComponent<PackageHolder>();
            if (body != null && body.DraggableBody.IsActive)
            {
                _selectedTrash = body;
                body.DraggableBody.FollowPointer(raycastCamera);
            }
        }
    }
}