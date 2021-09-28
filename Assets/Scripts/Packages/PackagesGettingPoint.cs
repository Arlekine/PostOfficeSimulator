using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PackagesGettingPoint : MonoBehaviour
{
    public Action<IPackageReceiver> OnNewPackageDesireGetted;
    public bool IsActive => _currentReceiver != null;
    
    [SerializeField] private Transform _packageGivingPoint;
    [SerializeField] private float _packageGivingOffset = 1f;
    [SerializeField] private float _packageDeletingOffset = 0.3f;
    [SerializeField] private ParticleSystem _packageGivingFXCorrect;
    [SerializeField] private ParticleSystem _packageGivingFXIncorrect;

    private IPackageReceiver _currentReceiver;

    public void GivePackage(PackageHolder body)
    {
        body.DraggableBody.IsActive = false;
        body.DraggableBody.MoveToPoint(_packageGivingPoint, 0f, setRotation: true).OnComplete(() => StartCoroutine(GivePackageToReceiver(body)));
    }

    public void SetPackageReceiver(IPackageReceiver receiver)
    {
        _currentReceiver = receiver;
        OnNewPackageDesireGetted?.Invoke(receiver);
    }

    private IEnumerator GivePackageToReceiver(PackageHolder package)
    {
        yield return new WaitForSecondsRealtime(_packageGivingOffset);

        bool isRight = _currentReceiver.GivePackage(package.Package);
        _currentReceiver = null;
        
        if (isRight)
        {
            _packageGivingFXCorrect.gameObject.SetActive(true);
            _packageGivingFXCorrect.Play();
        }
        else
        {
            _packageGivingFXIncorrect.gameObject.SetActive(true);
            _packageGivingFXIncorrect.Play();
        }
        
        yield return new WaitForSecondsRealtime(_packageDeletingOffset);
        
        package.IssuePackage();
        Destroy(package.gameObject);
    }
}

public interface IPackageReceiver
{
    Package GetDesire();
    bool GivePackage(Package package);
}
