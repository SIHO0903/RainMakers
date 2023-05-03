using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//상점 메뉴
public class ShopManager : MonoBehaviour
{
    [SerializeField] Image[] imageLevel;
    [SerializeField] TextMeshProUGUI textRequireCoin;
    [SerializeField] int requireCoin=5;
    public enum AbilityType {ExtraHealthS, ExtraDamageS, ExtraRegenerationS, ExtraFireRateS, ExtraShieldS, ExtraExpGetS }
    public AbilityType type;

    //Playerprefs초기화 및 불러오기
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Init"))
        {
            PlayerPrefs.SetInt(AbilityType.ExtraHealthS.ToString(), requireCoin);
            PlayerPrefs.SetInt(AbilityType.ExtraDamageS.ToString(), requireCoin);
            PlayerPrefs.SetInt(AbilityType.ExtraRegenerationS.ToString(), requireCoin);
            PlayerPrefs.SetInt(AbilityType.ExtraFireRateS.ToString(), requireCoin);
            PlayerPrefs.SetInt(AbilityType.ExtraShieldS.ToString(), requireCoin);
            PlayerPrefs.SetInt(AbilityType.ExtraExpGetS.ToString(), requireCoin);
            return;
        }

        switch (type)
        {
            case AbilityType.ExtraHealthS:                
                MaxLevelCheckUI(PlayerPrefs.GetInt(AbilityType.ExtraHealthS.ToString()));
                requireCoin = PlayerPrefs.GetInt(AbilityType.ExtraHealthS.ToString());
                break;
            case AbilityType.ExtraDamageS:       
                MaxLevelCheckUI(PlayerPrefs.GetInt(AbilityType.ExtraDamageS.ToString()));
                requireCoin = PlayerPrefs.GetInt(AbilityType.ExtraDamageS.ToString());
                break;
            case AbilityType.ExtraRegenerationS:
                MaxLevelCheckUI(PlayerPrefs.GetInt(AbilityType.ExtraRegenerationS.ToString()));
                requireCoin = PlayerPrefs.GetInt(AbilityType.ExtraRegenerationS.ToString());
                break;
            case AbilityType.ExtraFireRateS:
                MaxLevelCheckUI(PlayerPrefs.GetInt(AbilityType.ExtraFireRateS.ToString()));
                requireCoin = PlayerPrefs.GetInt(AbilityType.ExtraFireRateS.ToString());
                break;
            case AbilityType.ExtraShieldS:
                MaxLevelCheckUI(PlayerPrefs.GetInt(AbilityType.ExtraShieldS.ToString()));
                requireCoin = PlayerPrefs.GetInt(AbilityType.ExtraShieldS.ToString());
                break;
            case AbilityType.ExtraExpGetS:
                MaxLevelCheckUI(PlayerPrefs.GetInt(AbilityType.ExtraExpGetS.ToString()));
                requireCoin = PlayerPrefs.GetInt(AbilityType.ExtraExpGetS.ToString());
                break;
        }
    }
    //버튼 클릭시
    public void AbilityIncr()
    {
        //소지금 부족시 버튼반응x
        if (GameManager.Instance.curCoin<requireCoin)
        {
            Debug.Log("돈이 없습니다.");
            return;
        }
        PlayerPrefs.SetInt("Init", 1);
        GameManager.Instance.curCoin -= requireCoin;
        requireCoin += 5;
        AudioManager.instance.SFXPlayer("LevelUp");
        switch (type)
        {
            case AbilityType.ExtraHealthS:
                GameManager.Instance.player.extraHealth += 5;   
                PlayerPrefs.SetInt(AbilityType.ExtraHealthS.ToString(), requireCoin);
                break;
            case AbilityType.ExtraDamageS:
                GameManager.Instance.extraDamage += 0.4f;         
                PlayerPrefs.SetInt(AbilityType.ExtraDamageS.ToString(), requireCoin);
                break;
            case AbilityType.ExtraRegenerationS:
                GameManager.Instance.player.extraRegeneration += 1f;          
                PlayerPrefs.SetInt(AbilityType.ExtraRegenerationS.ToString(), requireCoin);
                break;
            case AbilityType.ExtraFireRateS:
                GameManager.Instance.extraFireRate -= 0.06f;       
                PlayerPrefs.SetInt(AbilityType.ExtraFireRateS.ToString(), requireCoin);
                break;
            case AbilityType.ExtraShieldS:
                GameManager.Instance.player.extraShield += 5;
                PlayerPrefs.SetInt(AbilityType.ExtraShieldS.ToString(), requireCoin);
                break;
            case AbilityType.ExtraExpGetS:
                GameManager.Instance.extraExpGet += 0.1f;
                PlayerPrefs.SetInt(AbilityType.ExtraExpGetS.ToString(), requireCoin );
                break;
        }
        MaxLevelCheckUI(requireCoin);

    }

    //업그레이드 스탯만렙시 버튼비활성화
    private void MaxLevelCheckUI(int requireCoin)
    {
        if (requireCoin >= 30)
        {
            textRequireCoin.text = "MAX";
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            textRequireCoin.text = requireCoin.ToString();
        }
        for (int i = 0; i < requireCoin/5-1; i++)
        {
            imageLevel[i].color = Color.red;
        }
    }
}
