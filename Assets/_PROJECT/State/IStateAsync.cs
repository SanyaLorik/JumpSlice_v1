using Cysharp.Threading.Tasks;

public interface IStateAsync
{
    UniTask Enter();

    UniTask Exit();
}