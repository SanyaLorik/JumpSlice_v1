using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; private set; }
    [SerializeField] private TextMeshPro _numberText;

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
    }
}