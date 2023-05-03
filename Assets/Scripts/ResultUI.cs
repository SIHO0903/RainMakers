using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//(사망시,클리어시)결과창 UI 
public class ResultUI : MonoBehaviour
{
    TextMeshProUGUI text;
    public enum ResultText { Kill, Time, Coin}
    public ResultText type;
    public static int resultCoin;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void OnEnable()
    {
        switch(type)
        {
                //킬수
            case ResultText.Kill:
                text.text = string.Format($"Kill : {GameManager.Instance.curKillCount,5}");
                break;
                //생존시간
            case ResultText.Time:
                text.text = string.Format($"Time : {Mathf.FloorToInt(GameManager.Instance.gameTime / 60):D2}:" +
                                                 $"{Mathf.FloorToInt(GameManager.Instance.gameTime % 60):D2}");
                break; 
                //킬수와 생존시간에 따라 코인지급
            case ResultText.Coin:
                resultCoin = Mathf.FloorToInt(GameManager.Instance.curKillCount / 10) + Mathf.FloorToInt(GameManager.Instance.gameTime / 5);
                text.text = string.Format($"Coin : {resultCoin,5}");

                break;

        }
    }
}
