using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("BGM")]
    public AudioClip bgmClip;       // 배경음악
    public float bgmVolume = 0.3f;  // BGM 볼륨 (0~1)

    [Header("SFX")]
    public AudioClip boosterClip;   // 부스터 소리
    public AudioClip explosionClip; // 폭발 소리
    public float sfxVolume = 0.5f;  // 효과음 볼륨

    private AudioSource bgmSource;      // BGM 전용
    private AudioSource sfxSource;      // 효과음 전용
    private AudioSource boosterSource;  // 부스터 전용 (루프)

    void Awake()
    {
        instance = this;

        // AudioSource 3개 생성 (BGM, 효과음, 부스터)
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        boosterSource = gameObject.AddComponent<AudioSource>();

        // BGM 설정
        bgmSource.clip = bgmClip;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // 부스터 설정 (한 번만 재생)
        boosterSource.volume = sfxVolume;
        boosterSource.loop = false;
        boosterSource.playOnAwake = false;

        // BGM 바로 시작! (타이틀 화면부터 나오게)
        PlayBGM();
    }

    // BGM 시작
    public void PlayBGM()
    {
        if (bgmClip != null && !bgmSource.isPlaying)
            bgmSource.Play();
    }

    // BGM 정지
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // 부스터 소리 한 번 재생
    public void PlayBooster()
    {
        if (boosterClip != null)
            boosterSource.PlayOneShot(boosterClip);
    }

    // 부스터 소리 끄기
    public void StopBooster()
    {
        if (boosterSource.isPlaying)
            boosterSource.Stop();
    }

    // 효과음 한 번 재생 (폭발 등)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
