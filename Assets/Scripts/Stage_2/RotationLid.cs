using DG.Tweening;
using UnityEngine;

public class RotationLid : SmallBoxLid
{
    [SerializeField] private Vector3 _openedRotation;
    [SerializeField] private float _rotationTime;
    
    public override Tween Open()
    {
        var seq = DOTween.Sequence();
        
        seq.Append(transform.DOLocalRotate(_openedRotation, _rotationTime));
        seq.SetEase(Ease.InOutCubic);

        return seq;
    }
}