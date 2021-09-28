using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClientController : MonoBehaviour, IPackageReceiver
{
    public Action<ClientController, bool> OnPackageRecieved;
    public Action<ClientController> OnRecievingCompleted;
    
    [SerializeField] private ClientMover _mover;
    [SerializeField] private ClientDesire _desire;
    [SerializeField] private ClientReaction _reaction;
    [SerializeField] private float _rightPackageReactionTime;
    [SerializeField] private float _wrongPackageReactionTime;

    public ClientMover Mover => _mover;
    public ClientDesire Desire => _desire;

    public Package GetDesire()
    {
        return Desire.CurrentDesire;
    }

    public bool GivePackage(Package package)
    {
        bool desireFulfilled = _desire.TryPerformDesireWithPackage(package);
        StartCoroutine(PackageGettingReaction(desireFulfilled));
        _desire.DesireView.Hide();
        
        return desireFulfilled;
    }

    private IEnumerator PackageGettingReaction(bool rightPackage)
    {
        _reaction.ShowReaction(rightPackage ? ClientReactionType.Happy : ClientReactionType.Angry);
        OnPackageRecieved?.Invoke(this, rightPackage);
        
        yield return new WaitForSecondsRealtime(rightPackage ? _rightPackageReactionTime : _wrongPackageReactionTime);
        
        
        OnRecievingCompleted?.Invoke(this);
    }
}
