using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�������� �پ��� ������ 3������ �������ϴ� ��ũ��Ʈ
public class AbilityHUD : MonoBehaviour
{
    //��ư���� 3������ ����
    Button[] buttons;
    int[] ran = new int[3]; // ���������� ���� ����
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

        //���� ����
        while (true)
        {
            for (int i = 0; i < ran.Length; i++)
            {
                ran[i] = Random.Range(0, buttons.Length);
            }
            if (ran[0] != ran[1] && ran[0] != ran[2] && ran[1] != ran[2])
                break;
        }

        //������ ���� ���ȹ�ư Ȱ��ȭ
        for (int i = 0; i < ran.Length; i++)
        {
            if (buttons[ran[i]].gameObject.GetComponent<Ability>().isMaxLevel) 
                buttons[buttons.Length - 1].gameObject.SetActive(true); //Ȱ��ȭ�� ���� ������ ������ �������մ� ��ư���� �����Ͽ� Ȱ��ȭ
            else
                buttons[ran[i]].gameObject.SetActive(true);

            ran[i] = 0; //�����ʱ�ȭ
        }
        //Debug.Log(ran1 + " / " + ran2 + " / " + ran3);   

    }

}
