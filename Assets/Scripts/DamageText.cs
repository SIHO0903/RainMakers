using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//������ �ؽ�Ʈ
public class DamageText : MonoBehaviour
{
    TextMeshPro text;
    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }
    //�ؽ�Ʈ�� ���������
    public void SetUp(float damage)
    {
        text.SetText(damage.ToString());
    }

    //�ؽ�Ʈ�� ��ġ, ��µ� ��������ġ
    public static DamageText Create(Vector3 position, float damage)
    {
        GameObject damageTextObj =  GameManager.Instance.poolManager.Get(5);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        damageTextObj.transform.position = position;
        damageText.SetUp(damage);
        return damageText;
    }
    //�ڿ������� ������� ����
    private void Update()
    {
        if(transform.localScale.x<=0.3)
            gameObject.SetActive(false);
    }
}
