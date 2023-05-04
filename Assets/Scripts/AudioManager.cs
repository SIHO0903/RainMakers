using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ https://opengameart.org/
//������ ����Ż �켭����ũ ����⿡�� ������ ��ũ��Ʈ�� �����Ѱ� ����
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] AudioClip[] bgm;
    [SerializeField] AudioClip[] sfx;

    //������ + ��Ÿ����
    enum BGM { Start,InGame,Boss};
    enum SFX { Dead, Hit0, Hit1, LevelUp, Lose, Range, Select, Win};
    SFX type;

    AudioSource[] audioSources; //10���� ������ҽ��� 0���� bgm, �������� sfx
    //option���� �����̴��� ���� ����
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;



    void Awake()
    {
        instance= this;
        audioSources = GetComponents<AudioSource>();

    }
    private void Start()
    {
        //PlayerPrefs�ʱ�ȭ �� �ҷ�����
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

    // ��������
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
        //��������� ���� ������ҽ��ΰ��� �÷���
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
    //�������� HitSFX ���
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
