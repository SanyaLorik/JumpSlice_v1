using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class StateBase : MonoBehaviour, IStateAsync
{
    public abstract UniTask Enter();

    public abstract UniTask Exit();
}
