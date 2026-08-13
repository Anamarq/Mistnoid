using UnityEngine;

public class AcidEnemy : MonoBehaviour
{
    [Header("Acid")]
    [SerializeField] private GameObject acidBallPrefab;
    [SerializeField] private Transform acidSpawnPoint;
    [SerializeField] private float acidInterval = 2f;

    private float acidTimer;

    private void Start()
    {
        acidTimer = acidInterval;
    }

    private void Update()
    {
        acidTimer -= Time.deltaTime;

        if (acidTimer <= 0f)
        {
            DropAcidBall();
            acidTimer = acidInterval;
        }
    }

    private void DropAcidBall()
    {
        if (acidBallPrefab == null)
            return;

        Instantiate(acidBallPrefab,acidSpawnPoint.position,Quaternion.identity);
    }
}