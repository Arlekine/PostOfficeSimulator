using DG.Tweening;
using UnityEngine;

public class SmallBox : MonoBehaviour
{
    [SerializeField] private SmallBoxLid _lid;

    [SerializeField] private bool _showContentEffect;
    [SerializeField] private Transform _content;
    [SerializeField] private ParticleSystem _contentAppearFX;
    [SerializeField] private Vector3 _openedPos;
    [SerializeField] private Vector3 _openedRotation;
    [SerializeField] private float _contentStartTime;
    [SerializeField] private float _contentTime;
    
    public Tween Open()
    {
        var seq = DOTween.Sequence();
        seq.Append(_lid.Open());

        if (_showContentEffect)
        {
            var seq2 = DOTween.Sequence();
            seq2.AppendInterval(_contentStartTime);
            seq2.AppendCallback(() =>
            {
                _contentAppearFX.Play();
            });
            seq2.Append(_content.DOLocalMove(_openedPos, _contentTime));
            seq2.Join(_content.DOLocalRotate(_openedRotation, _contentTime));
            seq2.Join(_content.DOScale(1f, _contentTime));

            seq.Join(seq2);
        }

        return seq;
    }

    public Tween MoveOut(Vector3 endPoint, float moveTime)
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(endPoint, moveTime));
        seq.Append(transform.DOLocalRotate(Vector3.zero, moveTime));

        return seq;
    }
}