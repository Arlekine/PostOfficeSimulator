using DG.Tweening;
using UnityEngine;

public class PulsingAnimation : MonoBehaviour
{
    [SerializeField] private float _bigScale;
    [SerializeField] private float _time = 0.3f;
    
    private Sequence _sequence;
    
    private void Start()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append((transform.DOScale(_bigScale, _time))).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}