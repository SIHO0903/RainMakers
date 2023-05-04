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
        //������ ũ�Ⱑ 0�� �ƴϸ� move�ִϸ��̼� ����
        anim.SetBool("Move", inputVec.magnitude != 0);
    }

    private void SpriteFilp()
    {
        //�Էµ� ������ x�� ���� ��������Ʈ �ø�
        if (inputVec.x < 0)
            sprite.flipX = true;
        else if (inputVec.x > 0)
            sprite.flipX = false;
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;
        Move(); //���������� Fixed�� ������ ���ٰ���
    }

    //�̵�
    private void Move()
    {
        //����� �������� ��Ÿ���ִ� GetAxisRaw�̿�
        inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //�ӵ����� velocity��� nomalized�� ������ ����ȭ(ũ�Ⱑ 1)
        rigid.velocity = inputVec.normalized * moveSpeed;
        
    }
    //ü�����
    private void Regeneration()
    {
        //0.2�ʸ��� ���, ������� ��ġ�� ���� �ݿ�
        regenerationTimer += Time.deltaTime;
        if (regenerationTimer > 0.2f && health <maxHealth) //0.2�ʸ��� ü�� ȸ��
        {
            health += curRegeneration /25;
            regenerationTimer = 0;
        }
    }
    //���� ���
    private void ShieldRegeneration()
    {
        //3�ʸ��� ���, ������� ��ġ�� ���� �ݿ�
        shieldRegenerationTime += Time.deltaTime;
        if (shieldRegenerationTime > 3f && shield < MaxShield) //curRegenerationTime���� ü�� ȸ��
        {
            shield += 3f*Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� ����ü�� ������
        if (collision.CompareTag("EnemyBullet") && isLive)
        {
            //������� ���
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
        //���� �浹��
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

    //���
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
    //�ִϸ��̼� �̺�Ʈ�� ����
    void SetFalse()
    {

        gameObject.SetActive(false);
    }

    //�浹�� ���� �Դ� �������� ƨ�ܳ���
    IEnumerator EnemyKnockBack(Collision2D collision)
    {
        collision.gameObject.GetComponent<Enemy>().isHit = true;
        Vector2 reactVec = collision.transform.position - transform.position;
        collision.rigidbody.AddForce(reactVec.normalized * 8f, ForceMode2D.Impulse);
        yield return wait;
        collision.gameObject.GetComponent<Enemy>().isHit = false;
    }
}