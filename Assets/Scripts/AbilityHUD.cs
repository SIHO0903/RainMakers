using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//레벨업시 다양한 스탯중 3가지만 나오게하는 스크립트
public class AbilityHUD : MonoBehaviour
{
    //버튼들중 3가지만 출현
    Button[] buttons;
    int[] ran = new int[3]; // 난수생성을 위한 변수
    void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    void OnEnable()
    {
        //Init
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        //난수 생성
        while (true)
        {
            for (int i = 0; i < ran.Length; i++)
            {
                ran[i] = Random.Range(0, buttons.Length);
            }
            if (ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2])
                break;
        }

        //난수에 따른 스탯버튼 활성화
        for (int i = 0; i < ran.Length; i++)
        {
            if (buttons[ran[i]].gameObject.GetComponent<Ability>().isMaxLevel) 
                buttons[buttons.Length - 1].gameObject.SetActive(true); //활성화될 스탯 만렙시 코인을 얻을수잇는 버튼으로 변경하여 활성화
            else
                buttons[ran[i]].gameObject.SetActive(true);

            ran[i] = 0; //난수초기화
        }
        //Debug.Log(ran1 + " / " + ran2 + " / " + ran3);   

    }

}
