using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiClientDesire : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Image _image;
    [SerializeField] private float _openedValue = 0.001f;
    [SerializeField] private float _openingTime = 0.5f;

    private void Awake()
    {
        _menu.transform.localScale = Vector3.zero;
    }
    
    public void SetIcon(Sprite icon)
    {
        _image.sprite = icon;
    }

    public void Show()
    {
        _menu.SetActive(true);
        _menu.transform.DOScale(_openedValue, _openingTime);
    }

    public void Hide()
    {
        _menu.transform.DOScale(0f, _openingTime);
    }
}