using UnityEngine;

public class DoctorGangbang : MonoBehaviour
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
        
        _firstPoliceman.Move(_firstPoint.position);
        _secondPoliceman.Move(_secondPoint.position);
    }

    public void MoveOut()
    {
        _firstPoliceman.moveSpeed = _moveOutSpeed;
        _secondPoliceman.moveSpeed = _moveOutSpeed;
        
        _firstPoliceman.Move(_firstMoveOutPoint.position);
        _secondPoliceman.Move(_secondMoveOutPoint.position);
    }
}