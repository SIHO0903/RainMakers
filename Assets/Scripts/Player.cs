using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("@ PlayerInfo")]
    [Header("# Health")]
    public float health;
    public float maxHealth;
    public float extraHealth;

    [Header("# Shield")]
    public float shield;
    public float MaxShield;
    public float extraShield;
    float shieldRegenerationTime;

    [Header("# Recovery")]
    public float curRegeneration;
    public float extraRegeneration;
    float regenerationTimer;

    [Header("# Move")]
    public Vector2 inputVec;
    public float moveSpeed;
    public bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    public Scanner scanner;
    public Spawner spawner;
    public Skill skill;
    WaitForSeconds wait;

    void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
        sprite= GetComponent<SpriteRenderer>();
        anim= GetComponent<Animator>();
        scanner = GetComponentInChildren<Scanner>();
        spawner = GetComponentInChildren<Spawner>();
        skill = GetComponentInChildren<Skill>();
        wait = new WaitForSeconds(0.15f);
    }
    private void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        shield = MaxShield;
    }
    private void Update()
    {
        if (!isLive)
            return;
        Regeneration();
        ShieldRegeneration();
        SpriteFilp();
        MoveAnim();
    }

    private void MoveAnim()
    {
        //벡터의 크기가 0이 아니면 move애니메이션 동작
        anim.SetBool("Move", inputVec.magnitude != 0);
    }

    private void SpriteFilp()
    {
        //입력된 벡터의 x에 따라 스프라이트 플립
        if (inputVec.x < 0)
            sprite.flipX = true;
        else if (inputVec.x > 0)
            sprite.flipX = false;
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;
        Move(); //물리연산은 Fixed에 넣으면 좋다고함
    }

    //이동
    private void Move()
    {
        //등속한 움직임을 나타내주는 GetAxisRaw이용
        inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //속도제어 velocity사용 nomalized로 벡터의 정규화(크기가 1)
        rigid.velocity = inputVec.normalized * moveSpeed;
        
    }
    //체력재생
    private void Regeneration()
    {
        //0.2초마다 재생, 재생력은 수치에 따라 반영
        regenerationTimer += Time.deltaTime;
        if (regenerationTimer > 0.2f && health <maxHealth) //0.2초마다 체력 회복
        {
            health += curRegeneration /25;
            regenerationTimer = 0;
        }
    }
    //쉴드 재생
    private void ShieldRegeneration()
    {
        //3초마다 재생, 재생력은 수치에 따라 반영
        shieldRegenerationTime += Time.deltaTime;
        if (shieldRegenerationTime > 3f && shield < MaxShield) //curRegenerationTime마다 체력 회복
        {
            shield += 3f*Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //적의 투사체에 맞을시
        if (collision.CompareTag("EnemyBullet") && isLive)
        {
            //쉴드재생 취소
            shieldRegenerationTime = 0;

            //Hit
            AudioManager.instance.RandomHitSFX();
            if (shield > 0)
            {
                shield -= collision.gameObject.GetComponent<EnemyBullet>().damage;
                if (shield < 0)
                {
                    health += shield;
                    shield = 0;
                }
                
            }
            else
            {
                health -= collision.gameObject.GetComponent<EnemyBullet>().damage;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //적과 충돌시
        if (collision.gameObject.CompareTag("Enemy") && isLive)
        {

            shieldRegenerationTime = 0;

            StartCoroutine(EnemyKnockBack(collision));
            //Hit
            AudioManager.instance.RandomHitSFX();
            if (shield > 0)
            {
                shield -= collision.gameObject.GetComponent<Enemy>().damage;
                if (shield < 0)
                {
                    health += shield;
                    shield = 0;
                }
            }
            else
            {
                health -= collision.gameObject.GetComponent<Enemy>().damage;
            }
            
            Dead();
        }
    }

    //사망
    void Dead()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            AudioManager.instance.SFXPlayer("Dead");
            AudioManager.instance.SFXPlayer("Lose");
            isLive = false;
            GameManager.Instance.Result();

        }
    }
    //애니메이션 이벤트로 연결
    void SetFalse()
    {

        gameObject.SetActive(false);
    }

    //충돌시 적이 왔던 방향으로 튕겨나감
    IEnumerator EnemyKnockBack(Collision2D collision)
    {
        collision.gameObject.GetComponent<Enemy>().isHit = true;
        Vector2 reactVec = collision.transform.position - transform.position;
        collision.rigidbody.AddForce(reactVec.normalized * 8f, ForceMode2D.Impulse);
        yield return wait;
        collision.gameObject.GetComponent<Enemy>().isHit = false;
    }
}