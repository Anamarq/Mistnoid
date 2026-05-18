using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource, loopSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    [Header("Audio Clips")]
    [SerializeField] private AudioClip musicMenu, musicGame, musicIntroGame;

    [SerializeField] private AudioClip sfxButton, sfxButtonBack, sfxPage, sfxCloseBook, sfxSouls, sfxFragments, 
        sfxBlockBreak, sfxBlocklv2, sfxBlockMetal, sfxShoot, sfxPaddle, sfxBallHit, sfxAbility,
        sfxWin, sfxLose, sfxBarHit, sfxWrong, sfxClickDialogue,
        sfxCat, sfxPhoenix, sfxFrog, sfxBirdWhite, sfxBirdBlack, sfxDragon, sfxNimboHappy, sfxNimboSad, sfxLife;

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
        if (loopSource.clip == clip) return;

        loopSource.clip = clip;
        loopSource.loop = loop;
        loopSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        loopSource.Stop();
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
    public void PlayMenuMusic()
    {
        musicSource.Stop();
        PlayMusic(musicMenu);
    }
    public void PlayGameMusic()
    {
        loopSource.Stop();
        StartCoroutine(GameMusicCoroutine());
    }
    bool musicPaused;
    IEnumerator GameMusicCoroutine()
    {
        musicSource.clip = musicIntroGame;
        musicSource.loop = false;
        musicSource.Play();

        loopSource.clip = musicGame;
        loopSource.loop = true;

        while (musicSource.time < musicSource.clip.length)
        {
            if (!musicPaused)
            {

            }

            yield return null;
        }

        loopSource.Play();
        musicSource.Stop();
    }
    public void PauseMusic()
    {
        musicPaused = true;
        musicSource.Pause();
        loopSource.Pause();
    }

    public void ResumeMusic()
    {
        musicPaused = false;
        musicSource.UnPause();
        loopSource.UnPause();
    }
    //SFX
    public void PlayWin()
    {
        PlaySFX(sfxWin);
    }
    public void PlayLose()
    {
        PlaySFX(sfxLose);
    }
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

    public void PlayAbility()
    {
        PlaySFX(sfxAbility);
    }
    public void PlayButtonBack()
    {
        PlaySFX(sfxButtonBack);
    }
    public void PlayWrong()
    {
        PlaySFX(sfxWrong);
    }
    public void PlayClickDialogue()
    {
        PlaySFX(sfxClickDialogue);
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
    public void PlayBarHit()
    {
        PlaySFX(sfxBarHit);
    }
    public void PlayCat()
    {
        PlaySFX(sfxCat);
    }
    public void PlayPhoenix()
    {
        PlaySFX(sfxPhoenix);
    }
    public void PlayBird()
    {
        PlaySFX(sfxBirdBlack);
    }
    public void PlayBirdW()
    {
        PlaySFX(sfxBirdWhite);
    }
    public void PlayDragon()
    {
        PlaySFX(sfxDragon);
    }
    public void PlayNimbo()
    {
        PlaySFX(sfxNimboHappy);
    }
    public void PlayNimboSad()
    {
        PlaySFX(sfxNimboSad);
    }
    public void PlayFrog()
    {
        PlaySFX(sfxFrog);
    }
    public void PlayLife()
    {
        PlaySFX(sfxLife);
    }
    #endregion
}