using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiHint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    public void Show(string text)
    {
        _text.text = text;
        _text.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _text.gameObject.SetActive(false);
    }
}