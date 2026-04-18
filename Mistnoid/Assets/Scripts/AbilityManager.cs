using UnityEngine;

public enum Ability
{
    None,
    Dragon,
    Frog,
    PhoenixFire,
    BlackBird,
    WhiteBird,
    Cat,
    Wopi,
    Heart
}

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;
    private Ability currentAbility = Ability.None;
    public Ability CurrentAbility => currentAbility;

    private int maxUses, currentUses;
    public int MaxUses => maxUses;  

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Load();
    }

    void Load()
    {
        currentAbility = (Ability)PlayerPrefs.GetInt("ActiveAbility", 0);
        maxUses = PlayerPrefs.GetInt("AbilityUses", 1);
        currentUses = maxUses;
    }

    public void SetAbility(Ability ability)
    {
        currentAbility = ability;
        PlayerPrefs.SetInt("ActiveAbility", (int)ability);
        PlayerPrefs.Save();
        Debug.Log("Habilidad activa: " + ability);
    }

    public bool CanUse()
    {
        return currentUses > 0;
    }

    public void Use()
    {
        currentUses--;
    }

    public void ResetUses()
    {
        currentUses = maxUses;
    }

    public void AddMaxUse()
    {
        maxUses++;
        PlayerPrefs.SetInt("AbilityUses", maxUses);
        PlayerPrefs.Save();
    }

}