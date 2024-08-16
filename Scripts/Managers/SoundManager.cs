using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum BGM
{
    Intro_Ending,
    Village,
    Dungeon,
    Boss,
    Dungeon2,
    BattleNPC,
    Boss2
}

public enum SoundEffects
{
    GoblinDie,
    DrawSword,
    GetItems,
    TypeDialogue,
    Hammer,
    SpearSkill,
}

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource bgmSource;
    private AudioSource sfxSource;
    [SerializeField] private AudioMixerGroup bgmAudioMixerGroup;
    [SerializeField] private AudioMixerGroup sfxAudioMixerGroup;
    [SerializeField] private AudioMixer masterMixer;

    [Header("BackGroundMusics")]
    public AudioClip[] Bgms;
    public float fadeDuration = 1.0f; // 사운드 페이드 인/아웃 시간
    [SerializeField][Range(0f, 1.0f)] private float bgmMaxVolume = 0.5f;
    [SerializeField][Range(0f, 1.0f)] private float sfxMaxVolume = 0.5f; // SFX 볼륨

    [Header("AttackSounds")]
    [SerializeField] AudioClip AttackSounds;

    [Header("SoundsEffects")]
    [SerializeField] AudioClip[] SoundEffects;

    private float initValue = -10f;

    private void Start()
    {
        // BGM과 SFX에 대한 오디오 소스를 각각 생성
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.outputAudioMixerGroup = bgmAudioMixerGroup;
        sfxSource.outputAudioMixerGroup = sfxAudioMixerGroup;

        masterMixer.SetFloat("Master", initValue);
        masterMixer.SetFloat("BGM", initValue);
        masterMixer.SetFloat("SFX", initValue);

        bgmSource.loop = true;  // BGM이 반복 재생되도록 설정
        SetBGM(Bgms[(int)BGM.Intro_Ending], false);
    }

    public void SetBGM(AudioClip newClip, bool shouldFade = true)
    {
        if (bgmSource.clip == newClip)
        {
            return; // 동일하면 재생을 생략
        }

        if (shouldFade)
        {
            StartCoroutine(FadeOutAndIn(newClip));
        }
        else // false라면 BGM이 바로 바뀌도록
        {
            bgmSource.clip = newClip;
            bgmSource.Play();
        }
    }

    private IEnumerator FadeOutAndIn(AudioClip newClip)
    {
        // 페이드 아웃
        float currentTime = 0;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(bgmMaxVolume, 0, currentTime / fadeDuration);
            yield return null;
        }

        bgmSource.volume = 0;
        bgmSource.clip = newClip;
        bgmSource.Play();

        // 페이드 인
        currentTime = 0;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, bgmMaxVolume, currentTime / fadeDuration);
            yield return null;
        }

        bgmSource.volume = bgmMaxVolume;
    }

    public void PlayAttackSound()
    {
        if(sfxSource.pitch != 1.0f)
        {
            sfxSource.pitch = 1.0f;
        }

        float randomPitch = Random.Range(0.5f, 1.3f);
        sfxSource.pitch = randomPitch;
        sfxSource.PlayOneShot(AttackSounds, sfxMaxVolume);

    }

    public void PlaySoundEffect(int idx, float volume = -1, int repeatCount = 1, bool setRndPitch = true)
    {
        if(setRndPitch)
        {
            float randomPitch = Random.Range(0.5f, 1.3f);
            sfxSource.pitch = randomPitch;
        }
        else sfxSource.pitch = 1.0f;

        if (volume < 0) volume = sfxMaxVolume;  // 기본 볼륨 설정
        StartCoroutine(PlaySoundEffectRepeatedly(idx, volume, repeatCount));
    }

    private IEnumerator PlaySoundEffectRepeatedly(int idx, float volume, int repeatCount)
    {
        sfxSource.pitch = 1.0f;

        for (int i = 0; i < repeatCount; i++)
        {
            sfxSource.PlayOneShot(SoundEffects[idx], volume);
            yield return new WaitWhile(() => sfxSource.isPlaying);
        }
    }

    public void PlaySkillSound(AudioClip clip, bool setRndPitch =true)
    {
        if (setRndPitch)
        {
            float randomPitch = Random.Range(0.5f, 1.3f);
            sfxSource.pitch = randomPitch;
        }
        else sfxSource.pitch = 1.0f;

        sfxSource.PlayOneShot(clip, sfxMaxVolume);
    }

    public void SetSceneBgm(int sceneNum)
    {
        if (sceneNum == (int)SceneNumber.DugeonScene_1)
        {
            GameManager.Instance.MainStageIdx = 1;
            SetBGM(Bgms[(int)BGM.Dungeon]);
        }
        else if (sceneNum == (int)SceneNumber.DugeonScene_2)
        {
            GameManager.Instance.MainStageIdx = 2;
            SetBGM(Bgms[(int)BGM.Dungeon2]);
        }
        else if (sceneNum == (int)SceneNumber.VillageScene)
        {
            SetBGM(Bgms[(int)BGM.Village]);
        }
        else if (sceneNum == (int)SceneNumber.EndScene)
        {
            SetBGM(Bgms[(int)BGM.Intro_Ending]);
        }
        else if (sceneNum == (int)SceneNumber.SelectSeene || sceneNum == (int)SceneNumber.StartScene) 
        { 
            SetBGM(Bgms[(int)BGM.Intro_Ending]);
        }
    }

    public void SetBossBgm()
    {
        switch (GameManager.Instance.MainStageIdx)
        {
            case 1:
                SetBGM(Bgms[(int)BGM.Boss]);
                break;
            case 2:
                SetBGM(Bgms[(int)BGM.Boss2]);
                break;
            default: break;
        }
    }

    public void SetCurDungeonBGM()
    {
        if(MoveStageController.isBossStage)
        {
            if (GameManager.Instance.MainStageIdx == 1)
            {
                SetBGM(Bgms[(int)BGM.Dungeon]);
            }
            else if (GameManager.Instance.MainStageIdx == 2)
            {
                SetBGM(Bgms[(int)BGM.Dungeon2]);
            }
        }
    }
}
