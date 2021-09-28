using DG.Tweening;
using UnityEngine;

public class UiAcceptionController : AcceptionController
{
    [SerializeField] private RectTransform _acceptButton;
    [SerializeField] private RectTransform _declineButton;

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
        _acceptButton.DOAnchorPos(_acceptButtonOpenedPos, _moveTime);
        _declineButton.DOAnchorPos(_declineButtonOpenedPos, _moveTime);
    }
    
    //from editor
    protected override void HideButtons()
    {
        _acceptButton.DOAnchorPos(_acceptButtonClosedPos, _moveTime);
        _declineButton.DOAnchorPos(_declineButtonClosedPos, _moveTime);
    }
}