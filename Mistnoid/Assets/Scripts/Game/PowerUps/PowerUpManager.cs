using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [SerializeField] private PowerUpData[] powerUps;
    [SerializeField] private GameObject powerUpPrefab;
    public GameObject PowerUpPrefab => powerUpPrefab;

    private HashSet<PowerUpType> unlocked = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PowerUpPool.Instance.Init();
        LoadUnlocked();
    }

    //UNLOCK SYSTEM
    #region Unlock

    void LoadUnlocked()
    {
        foreach (PowerUpData p in powerUps)
        {
            if (PlayerPrefs.GetInt(GetKey(p.type), 0) == 1)
            {
                unlocked.Add(p.type);
            }
        }
    }

    string GetKey(PowerUpType type)
    {
        return "PowerUp_" + type.ToString();
    }

    public void Unlock(PowerUpType type)
    {
        if (unlocked.Contains(type)) return;

        unlocked.Add(type);

        PlayerPrefs.SetInt(GetKey(type), 1);
        PlayerPrefs.Save();

        Debug.Log("PowerUp desbloqueado: " + type);
    }

    public bool IsUnlocked(PowerUpType type)
    {
        return unlocked.Contains(type);
    }



    #endregion

    // RANDOM  (only unlocked)

    PowerUpData GetRandomPowerUp()
    {
        List<PowerUpData> available = new();

        foreach (PowerUpData p in powerUps)
        {
            if (IsUnlocked(p.type))
                available.Add(p);
        }

        if (available.Count == 0)
        {
            Debug.LogWarning("No hay power ups desbloqueados");
            return null;
        }

        int totalWeight = 0;
        foreach (var p in available)
            totalWeight += p.weight;

        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var p in available)
        {
            currentWeight += p.weight;
            if (randomValue < currentWeight)
                return p;
        }

        return available[0];
    }

    void SpawnPowerUp(Vector3 pos, PowerUpData data)
    {
        if (data == null || !IsUnlocked(data.type)) return;

        GameObject obj = PowerUpPool.Instance.GetPowerUp();
        obj.transform.position = pos;

        PowerUp power = obj.GetComponent<PowerUp>();
        power.Init(data);
    }

    // EXTERNAL

    public void TrySpawnPowerUp(Vector3 position)
    {
        PowerUpData selected = GetRandomPowerUp();

        if (selected == null) return;

        SpawnPowerUp(position, selected);
    }
}