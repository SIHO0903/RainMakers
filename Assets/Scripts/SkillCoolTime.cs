using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//인게임에서 스킬 쿨타임을 가시적으로 보여주는 스크립트 
//쿨타임 계산방식을 0에서 증가가 아니라 해당 쿨타임에서 감소되는 방향이 더 좋아보임
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
