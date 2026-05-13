using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mañager of all the balls in the scene, controls number of balls, and the spawn of balls
public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    [SerializeField] private BallController ballPrefab;
    [SerializeField] private Transform paddle;
    private int maxBalls = 7;

    private List<BallController> activeBalls = new List<BallController>();

    //Power up fast/slow ball
    private float speedMultiplier = 1f;
    public float SpeedMultiplier => speedMultiplier;

    //power up invincible
    private Coroutine invincibleCoroutine;
    public bool IsInvActive = false;

    void Awake()
    {
        Instance = this;
    }

    #region Balls list control
    //Add new ball in the list
    public void RegisterBall(BallController ball)
    {
        if (!activeBalls.Contains(ball))
            activeBalls.Add(ball);
    }

    //Remove ball form the list. If there are no balls in the list, player lose a life
    public void RemoveBall(BallController ball)
    {
        activeBalls.Remove(ball);
        if (activeBalls.Count == 0)
        {
            PlayerController.Instance.LoseLife();
            Instantiate(ballPrefab);
            SetBlocksTrigger(false);
            IsInvActive = false;
        }
    }
    //Reset the list of the balls
    public void ResetBallsNextLevel()
    {
        foreach (BallController ball in activeBalls)
        {
            Destroy(ball.gameObject);
        }

        activeBalls.Clear();
        Instantiate(ballPrefab);
        ResetSpeed();
        SetBlocksTrigger(false);
        IsInvActive = false;
    }
    public void ResetSpeed()
    {
        speedMultiplier = 1f;
    }
    //Num of the balls in the level
    public int GetNumBalls()
    {
        return activeBalls.Count;
    }
    #endregion

    #region Power Ups
    //Multi ball power up
    //Returns the position of the first ball
    public Vector3 GetBallPosition()
    {
        if (activeBalls.Count > 0)
            return activeBalls[0].transform.position;
        else
        {
            Debug.LogError("THERE ARENT ANY BALLS");
            return paddle.position;
        }
    }

    public void SpawnExtraBalls(Vector3 position, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Debug.Log("Amount " + amount);
            if (GetNumBalls() < maxBalls)
            {
                BallController newBall = Instantiate(ballPrefab, position, Quaternion.identity);
                float angle = Random.Range(-60f, 60f);
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
                newBall.SetInvincibleVisual(true);
                newBall.Launch(dir);
            }
        }
    }

    //For the speed power up
    public void ModifySpeed(float multiplier)
    {
        speedMultiplier *= multiplier;
        speedMultiplier = Mathf.Clamp(speedMultiplier, 0.5f, 3f);// Clamp
    }

    //invencible powr up

    public void ActivateInvincible(float duration)
    {
        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);
        invincibleCoroutine = StartCoroutine(InvincibleRoutine(duration));
    }

    IEnumerator InvincibleRoutine(float duration)
    {
        IsInvActive = true;
        SetBlocksTrigger(true);
        foreach (var ball in activeBalls)
            ball.SetInvincibleVisual(true);
        yield return new WaitForSeconds(duration);
        SetBlocksTrigger(false);
        foreach (var ball in activeBalls)
            ball.SetInvincibleVisual(false);
        IsInvActive = false;
    }
    public void SetBlocksTrigger(bool state)
    {
        Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        foreach (var block in blocks)
        {
            block.SetTriggerMode(state);
        }
    }

    #endregion
}