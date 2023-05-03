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
        //생성시마다 초기화
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

    //이동
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

    //플레이어의 총알과 충돌시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.RandomHitSFX();
            stack++;
            stack = Mathf.Min(stack, maxStack); //스택은 최대4스택
            UICardStack();
            health -= collision.GetComponent<Bullet>().damage; //데미지 가격
            isHit = true;
            DamageText.Create(transform.position, GameManager.Instance.damage); //데미지텍스트 생성

            if (!bossDash)
            {
                Vector2 reactVec = transform.position - collision.transform.position;
                rigid.AddForce(reactVec.normalized * 8, ForceMode2D.Impulse);
            }
            Dead();
        }

    }



    // 코루틴으로 하면 오류가 발생해서 바꿈
    // Coroutine couldn't be started because the the game object 'enemy0'is inactive!
    // 이것저것해봣지만 해결되지않음
    public void IsHitFalse()
    {
        knockBackOff -= Time.deltaTime;
        if (knockBackOff < 0)
        {
            isHit = false;
            knockBackOff = 0.15f;
        }
    }

    //적 사망
    public void Dead()
    {
        if (health <= 0)
        {
            //보스만 해당
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

    //애니메이션 이벤트로 Dead와 연결
    private void SetFalse()
    {
        gameObject.SetActive(false);
    }

    //인스펙터에서 연결하지않고 스크립트상에서 컨트롤해보고싶어서 해봄
    //맞을때마다 스택이 증가하면서 머리위에 카드이미지가 하나씩 늘어남
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
