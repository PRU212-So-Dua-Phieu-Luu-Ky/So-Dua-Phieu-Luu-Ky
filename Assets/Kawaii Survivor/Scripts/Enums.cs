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
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalPercent,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRecoverySpeed,
    Armor,
    Luck,
    Dodge,
    LifeSteal
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        string formated = string.Empty;
        string unformatedString = stat.ToString();

        for (int i = 0; i < unformatedString.Length; i++)
        {
            if (i > 0 && char.IsUpper(unformatedString[i]))
                formated += ' ';

            formated += unformatedString[i];
        }

        return formated;
    }
}


