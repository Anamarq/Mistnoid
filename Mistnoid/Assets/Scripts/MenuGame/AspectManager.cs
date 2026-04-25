using UnityEngine;

public class AspectManager : MonoBehaviour
{
    public static AspectManager Instance;
    private const string BALL_SELECTED = "BallAspect_Selected";
    private const string BALL_UNLOCK = "BallAspect_";

    [SerializeField] private Sprite[] ballSprites;
    [SerializeField] private PaddleAspect[] paddleAspects;

    private int selectedPaddle = 0;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
            Destroy(gameObject);
    }

    void Init()
    {
        // Default desbloqueado
        if (PlayerPrefs.GetInt(BALL_UNLOCK + 0, 0) == 0)
            PlayerPrefs.SetInt(BALL_UNLOCK + 0, 1);
    }

    public void SetBallAspect(int index)
    {
        if (!IsBallUnlocked(index)) return;

        PlayerPrefs.SetInt(BALL_SELECTED, index);
        PlayerPrefs.Save();
    }

    public int GetBallAspect()
    {
        return PlayerPrefs.GetInt(BALL_SELECTED, 0);
    }

    public Sprite GetBallSprite()
    {
        return ballSprites[GetBallAspect()];
    }

    public bool IsBallUnlocked(int index)
    {
        return PlayerPrefs.GetInt(BALL_UNLOCK + index, 0) == 1;
    }

    public void UnlockBall(int index)
    {
        PlayerPrefs.SetInt(BALL_UNLOCK + index, 1);
        PlayerPrefs.Save();
    }

    public int GetRandomLockedBall()
    {
        System.Collections.Generic.List<int> locked = new();

        for (int i = 0; i < ballSprites.Length; i++)
        {
            if (!IsBallUnlocked(i))
                locked.Add(i);
        }

        if (locked.Count == 0) return -1;

        return locked[Random.Range(0, locked.Count)];
    }


    //PADDLE
    public Sprite GetPaddleSprite(int level)
    {
        var aspect = paddleAspects[selectedPaddle];

        if (level < aspect.spritesByLevel.Length)
            return aspect.spritesByLevel[level];

        return aspect.spritesByLevel[aspect.spritesByLevel.Length - 1];
    }

    public void SetPaddleAspect(int index)
    {
        selectedPaddle = index;
        PlayerPrefs.SetInt("PaddleAspect", index);
        PlayerPrefs.Save();
    }

    public int GetSelectedPaddle()
    {
        return selectedPaddle;
    }
}
[System.Serializable]
public class PaddleAspect
{
  //  public string name;
    public Sprite[] spritesByLevel;
}
