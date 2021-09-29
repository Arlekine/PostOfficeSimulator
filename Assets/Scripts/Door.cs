using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 _openedRotation;
    [SerializeField] private Vector3 _closedRotation;
    [SerializeField] private float _openingTime;

    private Tween _openingTween;

    public void Open()
    {
        _openingTween?.Kill();
        _openingTween = transform.DOLocalRotate(_openedRotation, _openingTime).SetEase(Ease.OutBounce);
    }
    
    public void Close()
    {
        _openingTween?.Kill();
        _openingTween = transform.DOLocalRotate(_closedRotation, _openingTime).SetEase(Ease.OutBounce);
    }
}
