using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [SerializeField] private PowerUpData[] powerUps;
    [SerializeField] private GameObject powerUpPrefab;
    public GameObject PowerUpPrefab { get { return powerUpPrefab; }  }


    private void Awake()
    {
        Instance = this;
        

    }

    private void Start()
    {
        PowerUpPool.Instance.Init();
    }


    PowerUpData GetRandomPowerUp()
    {
        int totalWeight = 0;
        foreach (PowerUpData p in powerUps)
            totalWeight += p.weight;
        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;
        foreach (PowerUpData p in powerUps)
        {
            currentWeight += p.weight;
            if (randomValue < currentWeight)
                return p;
        }
        return powerUps[0];
    }

    void SpawnPowerUp(Vector3 pos, PowerUpData data)
    {
        GameObject obj = PowerUpPool.Instance.GetPowerUp();
        obj.transform.position = pos;
        PowerUp power = obj.GetComponent<PowerUp>();
        power.Init(data);
    }

    //----------------------------------------- external calls
    public void TrySpawnPowerUp(Vector3 position)
    {
        PowerUpData selected = GetRandomPowerUp();
        SpawnPowerUp(position, selected);
    }
}