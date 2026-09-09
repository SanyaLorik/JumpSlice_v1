using Cysharp.Threading.Tasks;

public interface IPlatfromBonus
{
    bool HasBonus { get; }

    UniTask ApplyAsync();
}