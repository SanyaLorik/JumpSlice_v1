using TMPro;
using UnityEngine;

public class UiChance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;

    public void SetHealth(int health)
    {
        _healthText.text = health.ToString();
    }
}