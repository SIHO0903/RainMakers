using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� + ������ ����, �ҷ�����
public class AchiveManager : MonoBehaviour
{
    protected enum Achive { FirstKill, FirstBossKill, Kill10,BossKill2, Survive, Coin100}
    protected Achive[] achives;

    enum PlayerData 
    { 
        KillCount,BossKillCount, Coin,TotalCoin,
        ExtraDamage,ExtraExpGet,ExtraFireRate,ExtraHealth, ExtraShield, ExtraRegeneration
    }

    [SerializeField] GameObject uiNotice;
    WaitForSeconds wait;

    void Awake()
    { 
        //���� ȣ��Ǵ� �ڷ�ƾ�� ��� Awake���� �̸� �����ϸ� ���ɰ���(���α׷��� ������ ����)�� ����ȴ���
        wait = new WaitForSeconds(3);
        // Achive�� �ִ� �����͸� �迭�� Init
        achives = (Achive[])Enum.GetValues(typeof(Achive));

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }

    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("Save"))
            LoadData();
    }
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }
    //������ ����
    public static void SaveData()
    {
        PlayerPrefs.SetInt("Save", 1);

        PlayerPrefs.SetInt(PlayerData.KillCount.ToString(), GameManager.Instance.totalKillCount);
        PlayerPrefs.SetInt(PlayerData.BossKillCount.ToString(), GameManager.Instance.totalBossKillCount);
        PlayerPrefs.SetInt(PlayerData.Coin.ToString(), GameManager.Instance.curCoin);
        PlayerPrefs.SetInt(PlayerData.TotalCoin.ToString(), GameManager.Instance.totalCoin);

        PlayerPrefs.SetFloat(PlayerData.ExtraDamage.ToString(), GameManager.Instance.extraDamage);
        PlayerPrefs.SetFloat(PlayerData.ExtraExpGet.ToString(), GameManager.Instance.extraExpGet);
        PlayerPrefs.SetFloat(PlayerData.ExtraFireRate.ToString(), GameManager.Instance.extraFireRate);
        PlayerPrefs.SetFloat(PlayerData.ExtraHealth.ToString(), GameManager.Instance.player.extraHealth);
        PlayerPrefs.SetFloat(PlayerData.ExtraShield.ToString(), GameManager.Instance.player.extraShield);
        PlayerPrefs.SetFloat(PlayerData.ExtraRegeneration.ToString(), GameManager.Instance.player.extraRegeneration);
        //��ųʸ��� �̿��ϸ� �� �� �����Ҽ������Ű���
    }
    void LoadData()
    {
        GameManager.Instance.totalKillCount = PlayerPrefs.GetInt(PlayerData.KillCount.ToString());
        GameManager.Instance.totalBossKillCount = PlayerPrefs.GetInt(PlayerData.BossKillCount.ToString());
        GameManager.Instance.curCoin = PlayerPrefs.GetInt(PlayerData.Coin.ToString());
        GameManager.Instance.totalCoin = PlayerPrefs.GetInt(PlayerData.TotalCoin.ToString());

        GameManager.Instance.extraDamage = PlayerPrefs.GetFloat(PlayerData.ExtraDamage.ToString(), GameManager.Instance.extraDamage);
        GameManager.Instance.extraExpGet = PlayerPrefs.GetFloat(PlayerData.ExtraExpGet.ToString(), GameManager.Instance.extraExpGet);
        GameManager.Instance.extraFireRate = PlayerPrefs.GetFloat(PlayerData.ExtraFireRate.ToString(), GameManager.Instance.extraFireRate);
        GameManager.Instance.player.extraHealth = PlayerPrefs.GetFloat(PlayerData.ExtraHealth.ToString(), GameManager.Instance.player.extraHealth);
        GameManager.Instance.player.extraShield = PlayerPrefs.GetFloat(PlayerData.ExtraShield.ToString(), GameManager.Instance.player.extraShield);
        GameManager.Instance.player.extraRegeneration = PlayerPrefs.GetFloat(PlayerData.ExtraRegeneration.ToString(), GameManager.Instance.player.extraRegeneration);
    }
    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            //���� ����
            case Achive.FirstKill:
                isAchive = GameManager.Instance.totalKillCount >= 1;
                break;
            case Achive.FirstBossKill:
                isAchive = GameManager.Instance.totalBossKillCount==1;
                break;
            case Achive.Kill10:
                isAchive = GameManager.Instance.totalKillCount >= 10;
                break;
            case Achive.BossKill2:
                isAchive = GameManager.Instance.totalBossKillCount == 2;
                break;
            case Achive.Survive:
                isAchive = GameManager.Instance.gameTime > 90f;
                break;
            case Achive.Coin100:
                isAchive = GameManager.Instance.totalCoin>= 100;
                break;

        }
        //���� �޼���
        if(isAchive && PlayerPrefs.GetInt(achive.ToString())==0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            //ȭ�鿡 �˾�
            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.SFXPlayer("LevelUp");
        yield return wait;

        uiNotice.SetActive(false);

    }
}
