using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecondStageController : MonoBehaviour
{
    [SerializeField] private List<ClientController_Stage2> _clients = new List<ClientController_Stage2>();
    [SerializeField] private Door _door;
    [SerializeField] private float _doorCloseOffset;
    [SerializeField] private Transform _clientsSpawnPoint;
    [SerializeField] private Transform _clientsOutPoint;
    [SerializeField] private Transform _tablePoint;
    [SerializeField] private Transform _boxAcceptionPoint;
    [SerializeField] private UiHint _uiHint;

    [Space] 
    [SerializeField] private Transform _boxSpawnPoint;
    [SerializeField] private Transform _boxStayPoint;
    [SerializeField] private Transform _boxMoveOutPoint;

    [Space]
    [SerializeField] private float _firstBoxMoveTime;
    [SerializeField] private float _timeForEnding;
    [SerializeField] private float _nextClientOffset;

    [Space]
    [SerializeField] private AcceptionController _boxCheckingAcception;
    [SerializeField] private AcceptionController _finalCheckingAcception;
    [SerializeField] private PoliceGangbang _police;
    [SerializeField] private DoctorGangbang _doctors;

    [Space] 
    [SerializeField] private ResultPanel _resultPanel;
    [SerializeField] private string _winHeaderText;
    [SerializeField] private string _winScoreText;
    [SerializeField] private string _winButtonText;
    
    [Space] 
    [SerializeField] private string _looseHeaderText;
    [SerializeField] private string _looseScoreText;
    [SerializeField] private string _looseButtonText;
     
    private ClientController_Stage2 _currentPlayer;
    private SmallBox _currentSmallBox;
    private ClientBox _currentBox;
    private int _currentPlayerIndex = 0;
    
    private void Start()
    {
        MovePlayerToTable(0);
    }

    private void MovePlayerToTable(int playerIndex)
    {
        ClearCurrentPlayer();
        
        _currentPlayer = Instantiate(_clients[playerIndex], _clientsSpawnPoint);
        _currentPlayer.transform.localPosition = Vector3.zero;
        _currentPlayer.transform.localRotation = Quaternion.identity;
        
        _door.Open();

        var doorCloseSeq = DOTween.Sequence();
        doorCloseSeq.AppendInterval(_doorCloseOffset);
        doorCloseSeq.AppendCallback(() => _door.Close());

        _currentPlayer.Move(_tablePoint.position);
        
        _currentPlayer.Mover.OnMoveCompleted += () =>
        {
            _currentPlayer.Mover.OnMoveCompleted = null;
            _currentPlayer.ShowDesire();
            _currentBox = Instantiate(_currentPlayer.Box, _boxSpawnPoint);
            _currentBox.transform.localPosition = Vector3.zero;
            _currentBox.transform.localRotation = Quaternion.identity;
            
            _currentBox.MoveIn(_boxStayPoint.position, _firstBoxMoveTime).OnComplete(() =>
            {
                _boxCheckingAcception.GetAcception(
                    OnAccepted: () =>
                    {
                        _currentBox.MoveUp();
                        _currentPlayer.HideDesire();
                        _uiHint.Show("Swipe!");
                        LeanTouch.OnFingerSwipe += ScreenSwiped;
                    },
                    OnDecline: () =>
                    {
                        _currentPlayer.HideDesire();
                        _currentBox.MoveOut(_boxAcceptionPoint.position, _firstBoxMoveTime).OnComplete(() =>
                        {
                            _currentPlayer.Move(_clientsOutPoint.position);
                            
                            var waitSeq = DOTween.Sequence();
                            waitSeq.AppendInterval(_nextClientOffset);
                            waitSeq.AppendCallback(() =>
                            {
                                _resultPanel.Show(false, _looseScoreText, _looseHeaderText, _looseButtonText);
                            });
                        });
                    });
            });
        };
    }

    private void ScreenSwiped(LeanFinger finger)
    {
        if (finger.SwipeScreenDelta.y < 0)
        {
            _uiHint.Hide();
            LeanTouch.OnFingerSwipe -= ScreenSwiped;
            var boxMovingResult = _currentBox.OpenAndMoveOut(_boxMoveOutPoint.position);
            _currentSmallBox = boxMovingResult.Item2;
            boxMovingResult.Item1.OnComplete(() =>
            {
                _uiHint.Show("Tap!");
                LeanTouch.OnFingerTap += ScreenTapped;
            });
        }
    }

    private void ScreenTapped(LeanFinger finger)
    {
        LeanTouch.OnFingerTap -= ScreenTapped;
        _uiHint.Hide();
        _currentSmallBox.Open().OnComplete(() =>
        {
            _finalCheckingAcception.GetAcception(
                OnAccepted: () =>
                {
                    _currentPlayer.ShowDefeatReaction();
                    switch (_currentPlayer.DetainType)
                    {
                        case ClientDetainType.Doctor:

                            _doctors.CallThePolice();
                            break;
                        
                        case ClientDetainType.Police:

                            _police.CallThePolice();
                            break;
                    }

                    _currentSmallBox.MoveOut(_boxMoveOutPoint.position, _firstBoxMoveTime);

                    var waitSeq = DOTween.Sequence();
                    waitSeq.AppendInterval(_timeForEnding);
                    waitSeq.AppendCallback(() =>
                    {
                        switch (_currentPlayer.DetainType)
                        {
                            case ClientDetainType.Doctor:

                                _doctors.MoveOut();
                                break;
                        
                            case ClientDetainType.Police:

                                _police.MoveOut();
                                break;
                        }
                        
                        _currentPlayer.Move(_clientsOutPoint.position);
                        
                        var seq = DOTween.Sequence();
                        seq.AppendInterval(4f);
                        seq.AppendCallback(() =>
                        {
                            if (_currentPlayerIndex == _clients.Count - 1)
                                _resultPanel.Show(true, _winScoreText, _winHeaderText, _winButtonText);
                            else
                            {
                                _currentPlayerIndex++;
                                MovePlayerToTable(_currentPlayerIndex);
                            }
                        });
                    });
                },
                OnDecline: () =>
                {
                    _currentSmallBox.MoveOut(_boxAcceptionPoint.position, _firstBoxMoveTime).OnComplete(() =>
                    {
                        _currentPlayer.Move(_clientsOutPoint.position);
                        
                        var waitSeq = DOTween.Sequence();
                        waitSeq.AppendInterval(_nextClientOffset);
                        waitSeq.AppendCallback(() =>
                        {
                            _resultPanel.Show(false, _looseScoreText, _looseHeaderText, _looseButtonText);
                        });
                    });
                });
        });
    }

    private void ClearCurrentPlayer()
    {
        if (_currentPlayer != null)
            Destroy(_currentPlayer.gameObject);

        if (_currentBox != null)
            Destroy(_currentBox.gameObject);
        
        if (_currentSmallBox != null)
            Destroy(_currentSmallBox.gameObject);
    }
}