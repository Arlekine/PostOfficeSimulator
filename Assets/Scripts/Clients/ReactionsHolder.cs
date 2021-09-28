
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DATA/ClientsReactions", fileName = "ClientReactions")]
public class ReactionsHolder : ScriptableObject
{
    [Serializable]
    private class Reaction
    {
        public ClientReactionType Type;
        public ParticleSystem ReactionFX;
        public float fxLiveTime;
        public string AnimationTriiger;
        
    }

    [SerializeField] private List<Reaction> _reactions = new List<Reaction>();
    
    public void ShowReaction(ClientReactionType type, Transform fxPoint, Animator animator)
    {
        var reaction = _reactions.Find(x => x.Type == type);

        if (reaction != null)
        {
            var fx = Instantiate(reaction.ReactionFX, fxPoint);
            Destroy(fx, reaction.fxLiveTime);
            animator.SetTrigger(reaction.AnimationTriiger);
        }
    }
}