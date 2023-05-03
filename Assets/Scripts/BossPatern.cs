using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatern : MonoBehaviour
{

    [SerializeField] int randomSkill;
    [SerializeField] float skillTime;
    [SerializeField] float dashSpeed;
    [SerializeField] AttackLine attackLine;
    Vector3 disCheck; //거리체크
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
        //대쉬에 가중치를 둔 난수생성
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
    //대쉬준비
    IEnumerator DashReady()
    {
        skillTime = 0;
        enemy.bossDash = true; //대쉬동안 이동정지 플레이어 총알에의한 넉백 정지
        rigid.mass = 100; // 대쉬준비동안 플레이어와 충돌해도 적게 밀리게함
        rigid.velocity= Vector3.zero;
        attackLine.gameObject.SetActive(true); //트레일렌더러 활성화

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
    //toofar보다 멀면 빠르게 플레이어 근처로 이동
    void TooFar()
    {
        disCheck = GameManager.Instance.player.transform.position - transform.position;
        if (disCheck.magnitude > tooFar)
            rigid.velocity = disCheck.normalized * dashSpeed;
    }
    //플레이어를 향해 유도탄 발사
    void HomingFire()
    {
        skillTime = 0;
        GameObject homing = GameManager.Instance.poolManager.Get(7);
        homing.transform.position = transform.position;
    }
}
