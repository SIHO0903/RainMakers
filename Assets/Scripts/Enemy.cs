using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header(" Info")]
    public float health;
    public float maxHealth;
    public float damage;
    [SerializeField] float speed;
    public bool isHit;
    [SerializeField] bool isBoss;

    [Header("# Etc")]
    [SerializeField] float exp;
    public float knockBackOff=0.15f;
    [SerializeField] GameObject healthBar;
    Vector3 dirVec;
    public bool bossDash;

    [Header("# Card Stack")]
    public int stack = 0;
    string uiStackCheckId = "Stack";
    int maxStack = 4;
    RectTransform[] uiStack;

    Rigidbody2D rigid;
    Collider2D collider2D;
    SpriteRenderer sprite;
    Animator anim;

    void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        sprite= GetComponent<SpriteRenderer>();
        anim= GetComponent<Animator>();
        uiStack = GetComponentsInChildren<RectTransform>(true);
    }
    private void OnEnable()
    {
        //�����ø��� �ʱ�ȭ
        if (isBoss)
        {
            gameObject.GetComponent<BossPatern>().enabled = true;
        }
        healthBar.gameObject.SetActive(true);
        rigid.simulated = true;
        collider2D.enabled = true;
        health = maxHealth;
        UICardStack();
    }
    void FixedUpdate()
    {
        if (isHit)
        {
            IsHitFalse();
            return;
        }
        Move();

    }

    //�̵�
    private void Move()
    {
        if (!bossDash)
        {
            dirVec = (GameManager.Instance.player.transform.position - transform.position).normalized;
            rigid.velocity = dirVec * speed;
        }

        if(dirVec.x<0)
            sprite.flipX= true;
        else if(dirVec.x>0) 
            sprite.flipX= false;


    }

    //�÷��̾��� �Ѿ˰� �浹��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.RandomHitSFX();
            stack++;
            stack = Mathf.Min(stack, maxStack); //������ �ִ�4����
            UICardStack();
            health -= collision.GetComponent<Bullet>().damage; //������ ����
            isHit = true;
            DamageText.Create(transform.position, GameManager.Instance.damage); //�������ؽ�Ʈ ����

            if (!bossDash)
            {
                Vector2 reactVec = transform.position - collision.transform.position;
                rigid.AddForce(reactVec.normalized * 8, ForceMode2D.Impulse);
            }
            Dead();
        }

    }



    // �ڷ�ƾ���� �ϸ� ������ �߻��ؼ� �ٲ�
    // Coroutine couldn't be started because the the game object 'enemy0'is inactive!
    // �̰������ؔf���� �ذ��������
    public void IsHitFalse()
    {
        knockBackOff -= Time.deltaTime;
        if (knockBackOff < 0)
        {
            isHit = false;
            knockBackOff = 0.15f;
        }
    }

    //�� ���
    public void Dead()
    {
        if (health <= 0)
        {
            //������ �ش�
            if (isBoss)
            {
                GameManager.Instance.isBossDie = isBoss;
                GameManager.Instance.totalBossKillCount++;
                GameManager.Instance.curBossKillCount++;
                gameObject.GetComponent<BossPatern>().enabled = false;
                AudioManager.instance.BGMInGame();
            }

            anim.SetBool("Dead", true);

            GameManager.Instance.curExp += exp + GameManager.Instance.expGet;
            isHit = false;
            stack = 0;
            UICardStack();
            GameManager.Instance.totalKillCount++;
            GameManager.Instance.curKillCount++;
            rigid.simulated = false;
            collider2D.enabled = false;
            healthBar.gameObject.SetActive(false);
        }
    }

    //�ִϸ��̼� �̺�Ʈ�� Dead�� ����
    private void SetFalse()
    {
        gameObject.SetActive(false);
    }

    //�ν����Ϳ��� ���������ʰ� ��ũ��Ʈ�󿡼� ��Ʈ���غ���; �غ�
    //���������� ������ �����ϸ鼭 �Ӹ����� ī���̹����� �ϳ��� �þ
    public void UICardStack()
    {
        int count = 0;
        for (int i = 0; i < uiStack.Length; i++)
        {
            if (uiStack[i].name.Contains(uiStackCheckId))
            {
                uiStack[i].gameObject.SetActive(false);
                count++;
                if(count==stack)
                    uiStack[i].gameObject.SetActive(true); 
            }
        }
        
    }
}
