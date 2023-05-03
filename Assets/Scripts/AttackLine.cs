using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������� �뽬 ��θ� �����ִ� ��ũ��ư
public class AttackLine : MonoBehaviour
{
    [SerializeField] float lineSpeed;
    public Vector3 dashVec;
    WaitForSeconds wait;
    private void Awake()
    {
        wait = new WaitForSeconds(2f);
    }
    private void OnEnable()
    {
        //Ʈ���Ϸ������� ��ġ �ʱ�ȭ
        transform.position = transform.parent.position;
        //�뽬 ���� ����
        dashVec = GameManager.Instance.player.transform.position - transform.position;
    }
    private void Update()
    {
        //Ʈ���Ϸ����� �뽬��ΰ� ����� �ӵ�
        transform.Translate(dashVec.normalized * lineSpeed * Time.deltaTime);
        StartCoroutine(SetFalse());
    }
    IEnumerator SetFalse()
    {
        yield return wait;
        gameObject.SetActive(false);
    }
}
