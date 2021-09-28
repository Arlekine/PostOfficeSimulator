using DG.Tweening;
using UnityEngine;

public class ClientBox : MonoBehaviour
{
    [SerializeField] private SmallBox _internalBox;
    [SerializeField] private BoxSewAnimation _boxSewAnimation;
    
    [Header("Move Up params")]
    [SerializeField] private Vector3 _moveUpLocalPos;
    [SerializeField] private Vector3 _moveUpLocalRotation;
    [SerializeField] private float _moveUpTime;

    [Header("Opening Params")] 
    [SerializeField] private BoxLid _leftLid;
    [SerializeField] private BoxLid _rightLid;
    [SerializeField] private Vector3 _fallbackPos;
    [SerializeField] private float _fallbackTime;
    
    [Header("Small box Params")] 
    [SerializeField] private Vector3 _smallBoxMovePos;
    [SerializeField] private Vector3 _smallBoxMoveRotation;
    [SerializeField] private float _smallBoxTime;

    private Sequence _currentMoveSeq;

    public Tween MoveIn(Vector3 endPoint, float moveTime)
    {
        _currentMoveSeq?.Kill();
        _currentMoveSeq = DOTween.Sequence();

        _currentMoveSeq.Append(transform.DOMove(endPoint, moveTime).SetEase(Ease.OutBounce));

        return _currentMoveSeq;
    }

    public Tween MoveUp()
    {
        _currentMoveSeq?.Kill();
        _currentMoveSeq = DOTween.Sequence();

        _currentMoveSeq.Append(transform.DOLocalMove(_moveUpLocalPos, _moveUpTime));
        _currentMoveSeq.Join(transform.DOLocalRotate(_moveUpLocalRotation, _moveUpTime));

        return _currentMoveSeq;
    }

    public (Tween, SmallBox) OpenAndMoveOut(Vector3 endPoint)
    {
        _currentMoveSeq?.Kill();
        _currentMoveSeq = DOTween.Sequence();

        _currentMoveSeq.Append(_boxSewAnimation.Open());
        
        _currentMoveSeq.Append(_leftLid.Open());
        _currentMoveSeq.Join(_rightLid.Open());

        _internalBox.transform.parent = transform.parent;

        _currentMoveSeq.Append(transform.DOLocalMove(_fallbackPos, _fallbackTime).SetEase(Ease.InCubic));
        _currentMoveSeq.Append(transform.DOMove(endPoint, 1f).SetEase(Ease.OutCubic));
        
        _currentMoveSeq.Join(_internalBox.transform.DOLocalMove(_smallBoxMovePos, _smallBoxTime).SetEase(Ease.Linear));
        _currentMoveSeq.Join(_internalBox.transform.DOLocalRotate(_smallBoxMoveRotation, _smallBoxTime).SetEase(Ease.Linear));
        
        return (_currentMoveSeq, _internalBox);
    }

    public Tween MoveOut(Vector3 endPoint, float moveTime)
    {
        _currentMoveSeq?.Kill();
        _currentMoveSeq = DOTween.Sequence();

        _currentMoveSeq.Append(transform.DOMove(endPoint, moveTime));
        _currentMoveSeq.Append(transform.DOLocalRotate(Vector3.zero, moveTime));

        return _currentMoveSeq;
    }
}