using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레벨업시 3가지중 한가지 능력치 업그레이드를 선택하는 화면
public class Ability : MonoBehaviour
{
    //enum으로 제작자의 오타가능성 방지, 가독성 좋음
    public enum abilityInfo 
    { 
        MaxHealth, MaxShield, MoveSpeed,RecoverySpeed, Damage, GetExp, AttackSpeed,
        Blink,DoubleShot, Spade4, Homing, Coin
    }
    public abilityInfo type;

    //업그레이드한 능력이 만렙인지 확인
    public bool isMaxLevel;
    public void SelectAbility()
    {
        switch(type)
        {
            case abilityInfo.MaxHealth:
                GameManager.Instance.player.maxHealth += 5;
                break; 
            case abilityInfo.MaxShield:
                GameManager.Instance.player.MaxShield += 5;
                break;
            case abilityInfo.MoveSpeed:
                GameManager.Instance.player.moveSpeed += 0.3f;
                break;
            case abilityInfo.RecoverySpeed:
                GameManager.Instance.player.curRegeneration += 1;
                break;
            case abilityInfo.Damage:
                GameManager.Instance.damage += 1;
                break;
            case abilityInfo.GetExp:
                GameManager.Instance.expGet += 0.1f;
                break;
            case abilityInfo.AttackSpeed:
                GameManager.Instance.curFireRate *= 0.9f;
                break;
            case abilityInfo.Blink:
                GameManager.Instance.player.skill.blinkTempSpeed++;
                if(GameManager.Instance.player.skill.blinkTempSpeed==2)
                    isMaxLevel= true;
                break;
            case abilityInfo.DoubleShot:
                GameManager.Instance.player.skill.doubleShotDamageUp += 0.1f;
                if(GameManager.Instance.player.skill.doubleShotDamageUp==0.3)
                    isMaxLevel= true;
                break;
            case abilityInfo.Spade4:
                GameManager.Instance.player.skill.spade4ExtraDamage += 0.5f;
                if(GameManager.Instance.player.skill.spade4ExtraDamage==0.2f)
                    isMaxLevel = true;
                break;
            case abilityInfo.Homing:
                GameManager.Instance.player.skill.homingCount += 1;
                if (GameManager.Instance.player.skill.homingCount == 5)
                    isMaxLevel = true;
                break;
            case abilityInfo.Coin:
                GameManager.Instance.curCoin += 3;
                GameManager.Instance.totalCoin += 3;
                break;
        }
        //능력을 선택했으면 일시정지해제
        Time.timeScale = 1f;
        //스킬이나 일반공격 다시할수있게 true
        GameManager.Instance.canFire = true;
    }

}
//스크립터블 오브젝트를 활용해보자
