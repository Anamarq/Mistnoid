using UnityEngine;

public enum AchievementType
{
    CompleteBook,
    BlueMaster,
    RedMaster,
    PurpleMaster,
    BlackMaster,
    WhiteMaster,
    YellowMaster,
    GreenMaster,
    PinkMaster,
    WinInOneLife,
    WinFast,
    WinWithAbility,
    Escape,
    Score10000,
    Score50000
}
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    string GetKey(AchievementType type)
    {
        return "Achievement_" + type.ToString();
    }

    public bool IsUnlocked(AchievementType type)
    {
        return PlayerPrefs.GetInt(GetKey(type), 0) == 1;
    }

    public void Unlock(AchievementType type)
    {
        if (IsUnlocked(type)) return;

        PlayerPrefs.SetInt(GetKey(type), 1);
        PlayerPrefs.Save();

        Debug.Log("Logro desbloqueado: " + type);
    }
}