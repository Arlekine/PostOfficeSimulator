using System;
using System.Collections;
using UnityEngine;

public class ClientReaction : MonoBehaviour
{
    [SerializeField] private ReactionsHolder _reactions;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _reactionPoint;
    
    public void ShowReaction(ClientReactionType reactionType)
    {
        _reactions.ShowReaction(reactionType, _reactionPoint, _animator);
    }
}