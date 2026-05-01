using UnityEngine;

public static class ProgressFlags
{
    public static bool Level5Reached
    {
        get => PlayerPrefs.GetInt("Level5Reached", 0) == 1;
        set => PlayerPrefs.SetInt("Level5Reached", value ? 1 : 0);
    }

    public static bool Level5ImpossibleSeen
    {
        get => PlayerPrefs.GetInt("Level5ImpossibleSeen", 0) == 1;
        set => PlayerPrefs.SetInt("Level5ImpossibleSeen", value ? 1 : 0);
    }

    public static bool CatUnlocked
    {
        get => PlayerPrefs.GetInt("CatUnlocked", 0) == 1;
        set => PlayerPrefs.SetInt("CatUnlocked", value ? 1 : 0);
    }
}