using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatfromBonus
{
    [field: SerializeField] public Transform Target { get; private set; }
    [SerializeField] private TextMeshPro _numberText;

    [field: SerializeField] public bool HasBonus { get; private set; }

    public Vector3 Direction { get; private set; }

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public async UniTask ApplyAsync()
    {
        await BonusService.Instance.Large();
    }
}
