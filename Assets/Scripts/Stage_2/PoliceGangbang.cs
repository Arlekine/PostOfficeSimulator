using DG.Tweening;
using UnityEngine;

public class PoliceGangbang : MonoBehaviour
{
    [SerializeField] private float _callSpeed;
    [SerializeField] private float _moveOutSpeed;
    
    [Space]
    [SerializeField] private ClientMover _firstPoliceman;
    [SerializeField] private ClientMover _secondPoliceman;

    [Space] 
    [SerializeField] private Transform _firstPoint;
    [SerializeField] private Transform _secondPoint;
    
    [Space] 
    [SerializeField] private Transform _firstMoveOutPoint;
    [SerializeField] private Transform _secondMoveOutPoint;

    [Space] 
    [SerializeField] private Transform _cell;
    [SerializeField] private Vector3 _cellDownPos;
    [SerializeField] private Vector3 _cellUpPos;

    [Space] 
    [SerializeField] private GameObject _siren;

    private Vector3 _firstPolicemanInitialPos;
    private Vector3 _secondPolicemanInitialPos;

    private void Awake()
    {
        _firstPolicemanInitialPos = _firstPoliceman.transform.position;
        _secondPolicemanInitialPos = _secondPoliceman.transform.position;
    }

    public void CallThePolice()
    {
        _firstPoliceman.moveSpeed = _callSpeed;
        _secondPoliceman.moveSpeed = _callSpeed;
        
        _firstPoliceman.transform.position = _firstPolicemanInitialPos;
        _secondPoliceman.transform.position = _secondPolicemanInitialPos;
        
        _cell.DOMove(_cellDownPos, 0.5f).SetEase(Ease.OutBounce);
        _firstPoliceman.Move(_firstPoint.position);
        _secondPoliceman.Move(_secondPoint.position);
        _siren.SetActive(true);
    }

    public void MoveOut()
    {
        _firstPoliceman.moveSpeed = _moveOutSpeed;
        _secondPoliceman.moveSpeed = _moveOutSpeed;
        
        _cell.DOMove(_cellUpPos, 0.5f).SetEase(Ease.OutBounce);
        _firstPoliceman.Move(_firstMoveOutPoint.position);
        _secondPoliceman.Move(_secondMoveOutPoint.position);
        _siren.SetActive(false);
    }
}