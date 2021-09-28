using System;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))][SelectionBase]
public class DraggableBody : MonoBehaviour
{
    public bool IsActive { get; set; } = true;
    public bool IsDragging => _isFollowingPointer;

    [Range(0, 10)]
    [SerializeField] private float _maxFreeSpeed = 3f;
    
    private Rigidbody RigidBody 
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();

            return _rigidbody;
        }
    }
    
    private LayerMask _floorMask;
    private float _floorHeight = 2f;

    private Rigidbody _rigidbody;
    
    private Camera _pointerCamera;
    private bool _isFollowingPointer;

    private Vector2 _xMoveBorder;
    private Vector2 _zMoveBorder;

    private Sequence _moveTween;

    public void Init(LayerMask floorMask, float floorHeight)
    {
        _floorMask = floorMask;
        _floorHeight = floorHeight;
    }

    public void SetMoveBorder(Vector2 xBorder, Vector2 zBorder)
    {
        _xMoveBorder = xBorder;
        _zMoveBorder = zBorder;
    }

    public void Free()
    {
        _isFollowingPointer = false;
        RigidBody.useGravity = true;
        RigidBody.isKinematic = false;

        var maxPossibleSpeed = RigidBody.velocity.normalized * _maxFreeSpeed;
        RigidBody.velocity = RigidBody.velocity.magnitude > maxPossibleSpeed.magnitude ? maxPossibleSpeed : RigidBody.velocity;
    }
    
    public void FollowPointer(Camera pointerCamera)
    {
        _pointerCamera = pointerCamera;
        _isFollowingPointer = true;
        RigidBody.useGravity = false;
        RigidBody.isKinematic = true;
    }

    public Tween MoveToPoint(Transform point, float time, bool setRotation = false, bool freeAfterPath = false)
    {
        _isFollowingPointer = false;
        
        _moveTween?.Kill();
        _moveTween = DOTween.Sequence();
        
        _moveTween.Append(transform.DOMove(point.position, time));
        
        if (setRotation)
            _moveTween.Join(transform.DORotate(point.eulerAngles, time));

        if (freeAfterPath)
            _moveTween.AppendCallback(Free);

        return _moveTween;
    }

    private void FixedUpdate()
    {
        var pos = transform.position;
        
        if (_pointerCamera != null && _isFollowingPointer)
        {
            var fingers = LeanTouch.GetFingers(true, false);

            if (fingers.Count != 0)
            {
                RaycastHit hit;
                Ray ray = _pointerCamera.ScreenPointToRay(fingers[0].ScreenPosition);

                if (Physics.Raycast(ray, out hit, 1000f, layerMask: _floorMask))
                {
                    var cameraFloorDistance = hit.distance;
                    var cameraHeight = _pointerCamera.transform.position.y;
                    var cameraToBodyDistance =
                        cameraFloorDistance - (cameraFloorDistance * _floorHeight) / cameraHeight;

                    pos = _pointerCamera.ScreenToWorldPoint(new Vector3(fingers[0].ScreenPosition.x,
                        fingers[0].ScreenPosition.y, cameraToBodyDistance));
                }
            }
        }
        
        pos.x = Mathf.Clamp(pos.x, _xMoveBorder.x, _xMoveBorder.y);
        pos.z = Mathf.Clamp(pos.z, _zMoveBorder.x, _zMoveBorder.y);
            
        transform.position = (pos);
    }
}