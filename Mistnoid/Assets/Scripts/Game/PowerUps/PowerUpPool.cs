using System.Collections.Generic;
using UnityEngine;

//Pool fo the power ups

public class PowerUpPool : MonoBehaviour
{
    public static PowerUpPool Instance;

    private GameObject powerUpPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        
        powerUpPrefab = PowerUpManager.Instance.PowerUpPrefab;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(powerUpPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetPowerUp()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        GameObject newObj = Instantiate(powerUpPrefab);
        return newObj;
    }

    public void ReturnPowerUp(GameObject obj)
    {
        obj.SetActive(false);

        if (!pool.Contains(obj))
            pool.Enqueue(obj);
    }

    public void ResetPool()
    {
        PowerUp[] activePowerUps = FindObjectsByType<PowerUp>(FindObjectsSortMode.None);

        foreach (PowerUp p in activePowerUps)
        {
            ReturnPowerUp(p.gameObject);
        }
    }
}