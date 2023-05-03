using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�ΰ��ӿ��� ��ų ��Ÿ���� ���������� �����ִ� ��ũ��Ʈ 
//��Ÿ�� ������� 0���� ������ �ƴ϶� �ش� ��Ÿ�ӿ��� ���ҵǴ� ������ �� ���ƺ���
public class SkillCoolTime : MonoBehaviour
{
    enum SkillCool { Blink, DoubleShot, Spade4, Homing}
    [SerializeField] SkillCool skillCool;
    Image coolTimeImage;
    [SerializeField] Skill skill;
    void Awake()
    {
        coolTimeImage= GetComponentsInChildren<Image>()[1];
    }

    void Update()
    {
        switch(skillCool)
        {
            case SkillCool.Blink:
                coolTimeImage.fillAmount = 1 - skill.blinkTimer / skill.curblinkCool;
                break; 
            case SkillCool.DoubleShot:
                coolTimeImage.fillAmount = 1 - skill.doubleShotTimer / skill.curdoubleShotCool;
                break;
            case SkillCool.Spade4:
                coolTimeImage.fillAmount = 1 - skill.spade4Timer / skill.curSpade4Cool;
                break;
            case SkillCool.Homing:
                coolTimeImage.fillAmount = 1 - skill.homingTimer / skill.curhomingCool;
                break;
        }
    }
}
