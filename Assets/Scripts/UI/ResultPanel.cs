using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private CanvasGroup _back;
    [SerializeField] private RectTransform _textPanel;
    
    [Space]
    [SerializeField] private RectTransform _buttonRect;
    [SerializeField] private Vector2 _buttonRectOpenPos;
    [SerializeField] private Vector2 _buttonRecClosePos;
    
    [Space]
    [SerializeField] private RectTransform _headerRect;
    [SerializeField] private Vector2 _headerRectOpenPos;
    [SerializeField] private Vector2 _headerRectClosePos;

    [Space] 
    [SerializeField] private float _tweenTime;

    private int _sceneIndexToLoad;
    
    public void Show(bool win, string winScoreText, string headerText, string buttonText)
    {
        _scoreText.text = winScoreText;
        _headerText.text = headerText;
        _buttonText.text = buttonText;

        _sceneIndexToLoad = win ? GetNextSceneIndex() : SceneManager.GetActiveScene().buildIndex; 
        
        _back.DOFade(1f, _tweenTime);
        _textPanel.DOScale(1f, _tweenTime).SetEase(Ease.OutBack);
        _buttonRect.DOAnchorPos(_buttonRectOpenPos, _tweenTime).SetEase(Ease.OutCubic);
        _headerRect.DOAnchorPos(_headerRectOpenPos, _tweenTime).SetEase(Ease.OutCubic);
    }

    public void Hide()
    {
        _back.DOFade(0f, _tweenTime);
        _textPanel.DOScale(0f, _tweenTime);
    }

    public void SetScore(int score, int maxScore)
    {
        _scoreText.text = $"{score} / {maxScore}";
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneIndexToLoad);
    }

    private int GetNextSceneIndex()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        return currentScene < SceneManager.sceneCountInBuildSettings - 1 ? currentScene + 1 : 0;
    }
}
