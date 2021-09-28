using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClientMover : MonoBehaviour
{
    public Action OnMoveCompleted;
    
    public float moveSpeed = 5f;
    public float rotationTime = 0.3f;

    [Space]
    [SerializeField] private Animator _animator;

    private Sequence _moveSequence;

    public Tween GetCurrentMoveSequence()
    {
        return _moveSequence;
    }

    public Tween Move(params Vector3[] points)
    {
        _moveSequence?.Kill();
        _moveSequence = DOTween.Sequence();


        _animator.SetBool("Move", true);
        
        for (int i = 0; i < points.Length; i++)
        {
            var stepPosition = i == 0 ? transform.position : points[i - 1];
            var lookDirection = points[i] - stepPosition;
            
            var startPoint = i == 0 ? transform.position : points[i - 1];
            var distance = (points[i] - startPoint).magnitude;
            _moveSequence.Append(transform.DOMove(points[i], distance / moveSpeed).SetEase(Ease.Linear));
            _moveSequence.Join(transform.DORotate(Quaternion.LookRotation(lookDirection).eulerAngles, rotationTime).SetEase(Ease.Linear));
        }

        _moveSequence.AppendCallback(() =>
        {
            OnMoveCompleted?.Invoke();
            _animator.SetBool("Move", false);
        });
        
        return _moveSequence;
    }
}
