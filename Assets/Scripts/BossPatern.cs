using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatern : MonoBehaviour
{

    [SerializeField] int randomSkill;
    [SerializeField] float skillTime;
    [SerializeField] float dashSpeed;
    [SerializeField] AttackLine attackLine;
    Vector3 disCheck; //�Ÿ�üũ
    [SerializeField] float tooFar;
    Enemy enemy;
    Rigidbody2D rigid;
    WaitForSeconds wait;
    WaitForSeconds dashEndTime;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rigid = GetComponent<Rigidbody2D>();
        attackLine = GetComponentInChildren<AttackLine>(true);
        wait = new WaitForSeconds(2f);
        dashEndTime = new WaitForSeconds(0.3f);
    }
    private void Update()
    {
        
        skillTime += Time.deltaTime;
        //�뽬�� ����ġ�� �� ��������
        randomSkill = Random.Range(0, 5);
        if (skillTime > 5)
        {

            if (randomSkill == 0)
            {
                HomingFire();
            }
            else if (randomSkill >= 1)
            {
                StartCoroutine(DashReady());
            }

        }
        TooFar();
    }
    //�뽬�غ�
    IEnumerator DashReady()
    {
        skillTime = 0;
        enemy.bossDash = true; //�뽬���� �̵����� �÷��̾� �Ѿ˿����� �˹� ����
        rigid.mass = 100; // �뽬�غ񵿾� �÷��̾�� �浹�ص� ���� �и�����
        rigid.velocity= Vector3.zero;
        attackLine.gameObject.SetActive(true); //Ʈ���Ϸ����� Ȱ��ȭ

        yield return wait;
        rigid.mass = 1;
        Dash();
        yield return dashEndTime;
        enemy.bossDash = false;

    }
    void Dash()
    {
        rigid.velocity = attackLine.dashVec.normalized * dashSpeed;

    }
    //toofar���� �ָ� ������ �÷��̾� ��ó�� �̵�
    void TooFar()
    {
        disCheck = GameManager.Instance.player.transform.position - transform.position;
        if (disCheck.magnitude > tooFar)
            rigid.velocity = disCheck.normalized * dashSpeed;
    }
    //�÷��̾ ���� ����ź �߻�
    void HomingFire()
    {
        skillTime = 0;
        GameObject homing = GameManager.Instance.poolManager.Get(7);
        homing.transform.position = transform.position;
    }
}
