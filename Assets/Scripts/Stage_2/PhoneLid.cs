using DG.Tweening;
using UnityEngine;

public class PhoneLid : SmallBoxLid
{
    [SerializeField] private Vector3 _firstPoint;
    [SerializeField] private Vector3 _secondPoint;
    [SerializeField] private Vector3 _thirdPoint;
    [SerializeField] private float _pointTime;
    
    public override Tween Open()
    {
        var seq = DOTween.Sequence();
        
        seq.Append(transform.DOLocalMove(_firstPoint, _pointTime).SetEase(Ease.Linear));
        seq.Append(transform.DOLocalMove(_secondPoint, _pointTime).SetEase(Ease.Linear));
        seq.Append(transform.DOLocalMove(_thirdPoint, _pointTime).SetEase(Ease.Linear));
        seq.SetEase(Ease.InOutCubic);

        return seq;
    }
}