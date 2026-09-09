using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; private set; }
    [SerializeField] private TextMeshPro _numberText;

    public Vector3 Direction { get; private set; }

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }
}