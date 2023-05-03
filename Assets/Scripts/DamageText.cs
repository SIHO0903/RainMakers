using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//데미지 텍스트
public class DamageText : MonoBehaviour
{
    TextMeshPro text;
    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }
    //텍스트에 데미지출력
    public void SetUp(float damage)
    {
        text.SetText(damage.ToString());
    }

    //텍스트의 위치, 출력될 데미지수치
    public static DamageText Create(Vector3 position, float damage)
    {
        GameObject damageTextObj =  GameManager.Instance.poolManager.Get(5);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        damageTextObj.transform.position = position;
        damageText.SetUp(damage);
        return damageText;
    }
    //자연스럽게 사라지게 조정
    private void Update()
    {
        if(transform.localScale.x<=0.3)
            gameObject.SetActive(false);
    }
}
