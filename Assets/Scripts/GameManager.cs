using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [Header("# Component")]
    public Player player;
    public PoolManager poolManager;
    [SerializeField] GameObject abilitySelect;
    [SerializeField] TextMeshProUGUI curCoinText;
    [SerializeField] GameObject[] uiFlow;

    [Header("PlayerControl")]
    public float curExp;
    public float exp;
    public float expGet;
    public float extraExpGet;
    public float level;
    public float damage;
    public float extraDamage;
    public float fireRate;
    public float curFireRate;
    public float extraFireRate;
    public bool canFire = true;


    [Header("# GameControl")]
    public float gameTime;
    public int curCoin;
    public int totalCoin;
    public bool isBossDie;
    public int curKillCount;
    public int totalKillCount;
    public int curBossKillCount;
    public int totalBossKillCount;
    bool isStart;
    enum uiFlowInit {StartUI, InGameUI, ShopUI, AchiveUI, OptionUI, ClearUI, DeadUI }

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        uiFlow[(int)uiFlowInit.StartUI].gameObject.SetActive(true);
        AudioManager.instance.BGMSTART();
    }
    private void Update()
    {
        if (isStart)
        {
            gameTime += Time.deltaTime;
            LevelUp();
            if (isBossDie)
            {
                player.spawner.isSpawning = true;

                if (gameTime > 90)
                {
                    Clear();
                    isStart = false;
                }

            }
        }
        //shop메뉴에서 현재보유코인 표시
        curCoinText.text = string.Format($"Coin : {curCoin}");

    }
    //시작 할때마다 플레이어 스탯 초기화
    void Init()
    {
        isStart = false;
        gameTime = 0;
        level = 1;
        exp = 5;
        curExp = 0;
        expGet = 0+extraExpGet;
        damage = 5+extraDamage;
        curFireRate = 1.2f+extraFireRate;

        player.maxHealth = 50+player.extraHealth;
        player.MaxShield = 5+player.extraShield;
        player.curRegeneration = 4+player.extraRegeneration;

        player.skill.blinkTempSpeed = 0;
        player.skill.doubleShotDamageUp = 0;
        player.skill.spade4ExtraDamage = 0;
        player.skill.homingCount = 3;
        
        curKillCount = 0;
        curBossKillCount = 0;
    }

    //레벨업시
    public void LevelUp()
    {
        if (curExp >= exp)
        {
            level++;
            curExp -= exp;
            exp += 5;
            damage++;
            abilitySelect.SetActive(true);
            canFire = false;
            AudioManager.instance.SFXPlayer("LevelUp");
            Time.timeScale= 0f;

        }
    }
    //시작버튼
    public void BtnStart()
    {
        Init();
        uiFlow[(int)uiFlowInit.InGameUI].gameObject.SetActive(true);
        uiFlow[(int)uiFlowInit.StartUI].gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        isStart = true;
        Time.timeScale = 1f;
        AudioManager.instance.BGMInGame();
        AudioManager.instance.SFXPlayer("Select");
    }
    //상점버튼
    public void BtnShop()
    {
        uiFlow[(int)uiFlowInit.ShopUI].gameObject.SetActive(true);
        uiFlow[(int)uiFlowInit.StartUI].gameObject.SetActive(false);
        AudioManager.instance.SFXPlayer("Select");
    }
    //업적버튼
    public void BtnAchive()
    {
        uiFlow[(int)uiFlowInit.AchiveUI].gameObject.SetActive(true);
        uiFlow[(int)uiFlowInit.StartUI].gameObject.SetActive(false);
        AudioManager.instance.SFXPlayer("Select");
    }
    //메인메뉴+뒤로가기 버튼
    public void BtnMainMenu()
    {
        for (int i = 0; i < uiFlow.Length; i++)
        {
            uiFlow[i].gameObject.SetActive(false);
        }
        uiFlow[(int)uiFlowInit.StartUI].gameObject.SetActive(true);
        Init();
        player.gameObject.SetActive(false);
        PlayerPrefs.Save();
        AchiveManager.SaveData();
        AudioManager.instance.SFXPlayer("Select");

    }
    //시작BGM ClearUI,DeadUI 버튼에 추가함
    public void BtnBGMSTART()
    {
        AudioManager.instance.BGMSTART();
    }
    //옵션버튼
    public void BtnOption()
    {
        uiFlow[(int)uiFlowInit.OptionUI].gameObject.SetActive(true);
        uiFlow[(int)uiFlowInit.StartUI].gameObject.SetActive(false);
        AudioManager.instance.SFXPlayer("Select");
    }
    //결과표시
    public void Result()
    {
        isStart = false;
        Time.timeScale = 0;
        uiFlow[(int)uiFlowInit.DeadUI].gameObject.SetActive(true);
        uiFlow[(int)uiFlowInit.InGameUI].gameObject.SetActive(false);

        for (int i = 0; i < poolManager.transform.childCount; i++)
        {
            poolManager.transform.GetChild(i).gameObject.SetActive(false);
        }
        curCoin += ResultUI.resultCoin;
        totalCoin += ResultUI.resultCoin; //업적을 위해 추가함
        PlayerPrefs.Save();
        AchiveManager.SaveData();
    }

    //생존
    void Clear()
    {
        Result();
        AudioManager.instance.SFXPlayer("Win");
    }
}
