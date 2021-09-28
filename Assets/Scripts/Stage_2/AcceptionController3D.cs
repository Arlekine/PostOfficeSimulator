using DG.Tweening;
using UnityEngine;

public class AcceptionController3D : AcceptionController
{
    [SerializeField] private Transform _acceptButton;
    [SerializeField] private Transform _declineButton;

    [Space] 
    [SerializeField] private Vector3 _acceptButtonOpenedPos;
    [SerializeField] private Vector3 _acceptButtonClosedPos;
    [SerializeField] private Vector3 _declineButtonOpenedPos;
    [SerializeField] private Vector3 _declineButtonClosedPos;

    [Space] 
    [SerializeField] private float _moveTime;
    
    //from editor
    protected override void ShowButtons()
    {
        _acceptButton.DOMove(_acceptButtonOpenedPos, _moveTime);
        _declineButton.DOMove(_declineButtonOpenedPos, _moveTime);
    }
    
    //from editor
    protected override void HideButtons()
    {
        _acceptButton.DOMove(_acceptButtonClosedPos, _moveTime);
        _declineButton.DOMove(_declineButtonClosedPos, _moveTime);
    }
}