using System;
using UnityEngine;

public abstract class AcceptionController : MonoBehaviour
{
    protected Action _onAccepted;
    protected Action _onDecline;
    
    public void GetAcception(Action OnAccepted, Action OnDecline)
    {
        _onAccepted = OnAccepted;
        _onDecline = OnDecline;
        ShowButtons();
    }

    public void SetAccepted()
    {
        _onAccepted?.Invoke();
        ClearCallbacks();
    }
    
    public void SetDeclined()
    {
        _onDecline?.Invoke();
        ClearCallbacks();
    }

    private void ClearCallbacks()
    {
        _onAccepted = null;
        _onDecline = null;
        HideButtons();
    }

    protected abstract void ShowButtons();
    protected abstract void HideButtons();
}