using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class StateBase : MonoBehaviour, IStateAsync
{
    public abstract UniTask EnterAsync();

    public abstract UniTask ExitAsync();
}
