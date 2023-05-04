using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//파일은 https://opengameart.org/
//유투버 골드메탈 뱀서라이크 만들기에서 가져옴 몇몇스크립트도 따라한게 있음
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioClip[] bgm;
    [SerializeField] AudioClip[] sfx;

    //가독성 + 오타방지
    enum BGM { Start,InGame,Boss};
    enum SFX { Dead, Hit0, Hit1, LevelUp, Lose, Range, Select, Win};
    SFX type;

    AudioSource[] audioSources; //10개의 오디오소스중 0번은 bgm, 나머지는 sfx
    //option에서 슬라이더로 음량 조절
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;



    void Awake()
    {
        instance= this;
        audioSources = GetComponents<AudioSource>();

    }
    private void Start()
    {
        //PlayerPrefs초기화 및 불러오기
        if (!PlayerPrefs.HasKey("Volumset"))
            Init();
        else
        {
            bgmSlider.value = PlayerPrefs.GetFloat("bgmVolum");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolum");
        }
    }
    void Init()
    {
        PlayerPrefs.SetInt("Volumset", 1);
        PlayerPrefs.SetFloat("bgmVolum", 1f);
        PlayerPrefs.SetFloat("sfxVolum", 1f);
    }
    private void Update()
    {
        VolumSetting(bgmSlider.value, sfxSlider.value);
    }
    private void LateUpdate()
    {
        PlayerPrefs.SetFloat("bgmVolum", bgmSlider.value);
        PlayerPrefs.SetFloat("sfxVolum", sfxSlider.value);
    }

    // 볼륨조절
    private void VolumSetting(float bgm, float sfx)
    {
        audioSources[0].volume = bgm;
        for (int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].volume = sfx;
        }
    }
    public void BGMSTART()
    {
        audioSources[0].clip = bgm[(int)BGM.Start];
        audioSources[0].Play();
    }
    public void BGMInGame()
    {
        audioSources[0].clip = bgm[(int)BGM.InGame];
        audioSources[0].Play();
    }
    public void BGMBoss()
    {
        audioSources[0].clip = bgm[(int)BGM.Boss];
        audioSources[0].Play();
    }
    public void SFXPlayer(string sfxType)
    {
        //재생중이지 않은 오디오소스로가서 플레이
        type = (SFX)Enum.Parse(typeof(SFX),sfxType);
        for (int i = 1; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying)
                continue;
            else
            {
                audioSources[i].clip = sfx[(int)type];
                audioSources[i].Play();
                return;
            }
        }
    }
    //랜덤으로 HitSFX 재생
    public void RandomHitSFX()
    {
        int ran = UnityEngine.Random.Range(0, 2);
        string hit;
        if (ran == 0)
            hit = "Hit0";
        else
            hit = "Hit1";
        SFXPlayer(hit);
    }
}
