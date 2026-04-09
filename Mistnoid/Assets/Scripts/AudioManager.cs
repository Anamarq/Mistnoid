using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    [Header("Audio Clips")]
    [SerializeField] private AudioClip musicMenu;
    [SerializeField] private AudioClip musicGame;

    [SerializeField] private AudioClip sfxButton, sfxButtonBack, sfxPage, sfxCloseBook, sfxSouls, sfxFragments, 
        sfxBlockBreak, sfxBlocklv2, sfxBlockMetal, sfxShoot, sfxPowerUp, sfxPaddle, sfxBallHit,
        sfxCat, sfxPhoenix, sfxFrog, sfxBirdWhite, sfxBirdBlack, sfxDragon, sfxWopiHappy, sfxWopiSad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ApplyVolume();
    }

    //  MUSIC

    private void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }


    // SFX

    private void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }


    //  VOLUME


    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        ApplyVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat(SFX_KEY, value);
        ApplyVolume();
    }

    void ApplyVolume()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
    }

    #region PlaySounds
    //Play sounds (external calls)
    public void PlayBlockBreak()
    {
        PlaySFX(sfxBlockBreak);
    }

    public void PlayBlock2()
    {
        PlaySFX(sfxBlocklv2);
    }

    public void PlayBlockMetal()
    {
        PlaySFX(sfxBlockMetal);
    }

    public void PlayShoot()
    {
        PlaySFX(sfxShoot);
    }
    public void PlayButton()
    {
        PlaySFX(sfxButton);
    }
    public void PlayButtonBack()
    {
        PlaySFX(sfxButtonBack);
    }
    public void PlayPage()
    {
        PlaySFX(sfxPage);
    }
    public void PlayCloseBook()
    {
        PlaySFX(sfxCloseBook);
    }
    public void PlaySouls()
    {
        PlaySFX(sfxSouls);
    }
    public void PlayFragments()
    {
        PlaySFX(sfxFragments);
    }
    public void PlayPaddle()
    {
        PlaySFX(sfxPaddle);
    }
    public void PlayBallHit()
    {
        PlaySFX(sfxBallHit);
    }
    #endregion
}