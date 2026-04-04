using System.Collections.Generic;
using UnityEngine;
using static ShopPanel;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    private Dictionary<UpgradeType, int> upgrades = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        LoadAll();
    }

    string GetKey(UpgradeType type)
    {
        return "Upgrade_" + type.ToString();
    }

    void LoadAll()
    {
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
        {
            int value = PlayerPrefs.GetInt(GetKey(type), 0);
            upgrades[type] = value;
        }
    }

    public int GetLevel(UpgradeType type)
    {
        return upgrades[type];
    }

    public void SetLevel(UpgradeType type, int level)
    {
        upgrades[type] = level;
        PlayerPrefs.SetInt(GetKey(type), level);
        PlayerPrefs.Save();
    }
}