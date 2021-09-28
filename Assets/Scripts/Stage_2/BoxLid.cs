using DG.Tweening;
using UnityEngine;

public class BoxLid : MonoBehaviour
{
    [SerializeField] private Vector3 _openedRotation;
    [SerializeField] private Vector3 _closedRotation;
    [SerializeField] private float _openingTime;

    public Tween Open()
    {
        return transform.DOLocalRotate(_openedRotation, _openingTime);
    }
    
    public Tween Close()
    {
        return transform.DOLocalRotate(_closedRotation, _openingTime);
    }
}