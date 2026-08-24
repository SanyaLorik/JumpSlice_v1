using Architecture_M;
using LuringPlayer_M;
using MediaKit_M.SkinChanger;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture_M/Localization/Localization Data")]
public class LocalizationData : LocalizationDataBase,
    IDailyRewardLocalization, ISkinChangerLocalization
{
    [field: Header("Статический текст")]
    [field: SerializeField] public StaticTranslation<string>[] StaticTranslates { get; private set; }

    public DailyRewardLocaliation DailyReward;
    public SkinChangerLocalization SkinChanger;

    DailyRewardLocaliation IDailyRewardLocalization.DailyReward => DailyReward;

    SkinChangerLocalization ISkinChangerLocalization.SkinChanger => SkinChanger;
}