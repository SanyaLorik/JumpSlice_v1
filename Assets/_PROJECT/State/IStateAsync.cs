using Cysharp.Threading.Tasks;

public interface IStateAsync
{
    UniTask EnterAsync();

    UniTask ExitAsync();
}