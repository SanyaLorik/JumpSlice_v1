using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiChance : MonoBehaviour
{
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _returnAdButton;
    [SerializeField] private Button _skipButton;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void SetHealth(int health)
    {
        _healthText.text = health.ToString();
    }

    public void AddReturnButtonListner(Action action)
    {
        _returnButton.onClick.AddListener(action.Invoke);
    }

    public void AddReturnAdButtonListner(Action action)
    {
        _returnAdButton.onClick.AddListener(action.Invoke);
    }

    public void AddSkipButtonListner(Action action)
    {
        _skipButton.onClick.AddListener(action.Invoke);
    }

    public void InteractReturnButton()
    {
        _returnButton.interactable = true;
    }

    public void UninteractReturnButton()
    {
        _returnButton.interactable = false;
    }
}