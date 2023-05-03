using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//인게임에서 표시되는 UI
public class HUD : MonoBehaviour
{
    enum InfoType { Time,Coin,Level, Health,HealthText,Shield, Exp,EnemyHealth, BossHealth,BossHealthText}
    [SerializeField] InfoType type;

    TextMeshProUGUI text;
    Slider slider;
    void Awake()
    {
        text= GetComponent<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch(type)
        {
            case InfoType.Time:
                text.text = string.Format($"{Mathf.FloorToInt(GameManager.Instance.gameTime / 60):D2}:" +
                                          $"{Mathf.FloorToInt(GameManager.Instance.gameTime % 60):D2}");
                break;
            case InfoType.Coin:
                text.text = string.Format($"{GameManager.Instance.curCoin}");
                break;
            case InfoType.Level:
                text.text = string.Format($"Lv.{GameManager.Instance.level}");
                break;
            case InfoType.Health:
                slider.value = GameManager.Instance.player.health / GameManager.Instance.player.maxHealth;
                break;
            case InfoType.HealthText:
                text.text = string.Format($"{Mathf.FloorToInt(GameManager.Instance.player.health+ GameManager.Instance.player.shield)}/" +
                    $"{GameManager.Instance.player.maxHealth}");
                break;
            case InfoType.Shield:
                slider.value = GameManager.Instance.player.shield / GameManager.Instance.player.MaxShield;
                break;
            case InfoType.Exp:
                slider.value = GameManager.Instance.curExp / GameManager.Instance.exp;
                break;
            case InfoType.EnemyHealth:
                float health = transform.parent.parent.GetComponent<Enemy>().health;
                float maxHealth = transform.parent.parent.GetComponent<Enemy>().maxHealth;
                slider.value = health/maxHealth;
                break;
            case InfoType.BossHealth:
                float bossHealth = transform.parent.parent.GetComponent<Enemy>().health;
                float bossMaxHealth = transform.parent.parent.GetComponent<Enemy>().maxHealth;
                slider.value = bossHealth / bossMaxHealth;
                break;
            case InfoType.BossHealthText:
                bossHealth = transform.parent.parent.parent.GetComponent<Enemy>().health;
                bossMaxHealth = transform.parent.parent.parent.GetComponent<Enemy>().maxHealth;
                text.text = string.Format($"Dragon [ {Mathf.FloorToInt(bossHealth)}/{bossMaxHealth} ]");
                break;


        }
    }
}