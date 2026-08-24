using Architecture_M;
using LuringPlayer_M;
using MediaKit_M.SkinChanger;
using System;

[Serializable]
public class GameSave : GameSaveBase,
    IDailyRewardSaveLoader, IWheelFortuneSaveLoader, IDailyQuestSaveLoader, ISkinSaveLoader, ICommunitySaveLoader
{
    public DailyRewardSave DailyRewardSave;
    public WheelFortuneSave WheelFortuneSave;
    public DailyQuestSave DailyQuestSave;
    public SkinSave SkinSave;
    public CommunitySave CommunitySave;
    
    public DailyRewardSave Load()
    {
        return DailyRewardSave;
    }

    WheelFortuneSave IWheelFortuneSaveLoader.Load()
    {
        return WheelFortuneSave;
    }

    DailyQuestSave IDailyQuestSaveLoader.Load()
    {
        return DailyQuestSave;
    }

    SkinSave ISkinSaveLoader.Load()
    {
        return SkinSave;
    }

    CommunitySave ICommunitySaveLoader.Load()
    {
        return CommunitySave;
    }
}