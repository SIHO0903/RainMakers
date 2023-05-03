using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//(�����,Ŭ�����)���â UI 
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
                //ų��
            case ResultText.Kill:
                text.text = string.Format($"Kill : {GameManager.Instance.curKillCount,5}");
                break;
                //�����ð�
            case ResultText.Time:
                text.text = string.Format($"Time : {Mathf.FloorToInt(GameManager.Instance.gameTime / 60):D2}:" +
                                                 $"{Mathf.FloorToInt(GameManager.Instance.gameTime % 60):D2}");
                break; 
                //ų���� �����ð��� ���� ��������
            case ResultText.Coin:
                resultCoin = Mathf.FloorToInt(GameManager.Instance.curKillCount / 10) + Mathf.FloorToInt(GameManager.Instance.gameTime / 5);
                text.text = string.Format($"Coin : {resultCoin,5}");

                break;

        }
    }
}
