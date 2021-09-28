using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _scoreChangeText;
    [SerializeField] private RectTransform _scoreChangeRect;
    [SerializeField] private CanvasGroup _scoreChangeCanvasGroup;

    [Space] 
    [SerializeField] private float _scoreChangeTweenTime;
    [SerializeField] private float _scoreChangeWaitTime = 3f;
    [SerializeField] private float _scoreBoopSize = 1.2f;
    [SerializeField] private float _scoreBoopTime = 0.15f;

    private int _currentScore = 0;
    private Sequence _currentTween;

    public void SetScore(int newScore)
    {
        int scoreChange = newScore - _currentScore;

        _scoreText.text = newScore.ToString();
        _scoreChangeText.text = scoreChange >= 0 ? "+" + scoreChange.ToString() : scoreChange.ToString();

        _currentTween?.Kill();
        _currentTween = DOTween.Sequence();

        _scoreChangeRect.localScale = Vector3.zero;
        _scoreChangeCanvasGroup.alpha = 0f;

        _currentTween.Append(_scoreChangeRect.DOScale(_scoreBoopSize, _scoreBoopTime).SetEase(Ease.OutBack));
        _currentTween.Join(_scoreChangeCanvasGroup.DOFade(1f, _scoreBoopTime * 0.3f));
        _currentTween.AppendInterval(_scoreChangeWaitTime);

        _currentTween.Append(_scoreChangeCanvasGroup.DOFade(0f, _scoreChangeTweenTime));

        _currentScore = newScore;
    }
}
