using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    public UnityEvent OnClick;

    [SerializeField] private Transform _buttonTrans;
    [SerializeField] private Vector3 _downPos;
    
    private void OnMouseDown()
    {
        var seq = DOTween.Sequence();
        seq.Append(_buttonTrans.DOLocalMove(_downPos, 0.15f).SetRelative());
        seq.Append(_buttonTrans.DOLocalMove(-_downPos, 0.15f).SetRelative());
        seq.AppendCallback(() =>
        {
            OnClick?.Invoke(); 
        });
    }
}