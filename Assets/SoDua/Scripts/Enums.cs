using System;
using System.ComponentModel;
using static UnityEngine.Rendering.DebugUI;

public enum GameState
{
    MENU,
    GAME,
    WAVE_TRANSITION,
    SHOP,
    WEAPON_SELECTION,
    GAME_OVER,
    STAGE_COMPLETE
}
public enum Stat
{
    [Description("Sát thương")]
    Attack,
    [Description("Tốc đánh")]
    AttackSpeed,
    [Description("Chí mạng (%)")]
    CriticalChance,
    [Description("ST chí mạng")]
    CriticalPercent,
    [Description("Tốc chạy")]
    MoveSpeed,
    [Description("Máu tối đa")]
    MaxHealth,
    [Description("Tầm đánh")]
    Range,
    [Description("Hồi phục máu")]
    HealthRecoverySpeed,
    [Description("Giáp")]
    Armor,
    [Description("May mắn")]
    Luck,
    [Description("Né")]
    Dodge,
    [Description("Hút máu")]
    LifeSteal
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        var field = stat.GetType().GetField(stat.ToString());

        if (field == null)
            return stat.ToString();

        var attribute = Attribute.GetCustomAttribute(field,
        typeof(DescriptionAttribute)) as DescriptionAttribute;

        return attribute == null ? stat.ToString() : attribute.Description;

        //string formated = string.Empty;
        //string unformatedString = stat.ToString();

        //for (int i = 0; i < unformatedString.Length; i++)
        //{
        //    if (i > 0 && char.IsUpper(unformatedString[i]))
        //        formated += ' ';

        //    formated += unformatedString[i];
        //}

        //return formated;
    }
}


