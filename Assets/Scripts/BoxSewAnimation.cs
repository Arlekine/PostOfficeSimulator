using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxSewAnimation : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Transform _knife;
    [SerializeField] private Vector3 _knifeStartPos;
    [SerializeField] private Vector3 _knifeEndPos;

    public Tween Open()
    {
        _skinnedMeshRenderer.gameObject.SetActive(true);
        _knife.gameObject.SetActive(true);

        var seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => 0f, x =>
            {
                if (x < 7f)
                {
                    _knife.localPosition = Vector3.Lerp(_knifeStartPos, _knifeEndPos, x / 7f);

                    int wholePart = (int) Math.Truncate((decimal) x);
                    int blendShapeIndex = 6 - wholePart;
                    float fracPart = x - wholePart;
                    _skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, fracPart);
                }

            }, 7f, 1f).SetEase(Ease.InExpo));

        seq.AppendCallback(() =>
        {
            _skinnedMeshRenderer.gameObject.SetActive(false);
            _knife.gameObject.SetActive(false);
        });
        
        return seq;
    }
}
