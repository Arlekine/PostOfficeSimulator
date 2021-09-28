using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController_Stage2 : MonoBehaviour
{
    public ClientMover Mover => _mover;
    public ClientBox Box => _package;

    public ClientDetainType DetainType;
    
    [SerializeField] private ClientMover _mover;
    [SerializeField] private UiClientDesire _desireUI;
    [SerializeField] private ClientBox _package;
    [SerializeField] private Animator _animator;

    public void ShowDefeatReaction()
    {
        _animator.SetBool("Angry", true);
    }

    public void Move(params Vector3[] movePoints)
    {
        _animator.SetBool("Angry", false);
        _mover.Move(movePoints);
    }

    public void ShowDesire()
    {
        _desireUI.Show();
    }

    public void HideDesire()
    {
        _desireUI.Hide();
    }
}

public enum ClientDetainType
{
    Police,
    Doctor
}