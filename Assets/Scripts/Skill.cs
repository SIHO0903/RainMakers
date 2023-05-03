using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("# Blink")]
    [SerializeField] float Blinkrange;
    [SerializeField] bool isBlock;
    public float blinkTimer;
    public float curblinkCool;
    public int blinkTempSpeed;

    [Header("# Double Shot")]
    public float doubleShotTimer;
    public float curdoubleShotCool;
    public float doubleShotDamageUp;

    [Header("# Spade4")]
    public float spade4Timer;
    public float curSpade4Cool;
    public float spade4ExtraDamage;
    RaycastHit2D[] aroundenemys;
    [SerializeField] float scanrange;
    [SerializeField] LayerMask layerMask;

    [Header("# Homing")]
    public float homingTimer;
    public float curhomingCool;
    public float homingCount;
    public bool canHoming;


    Player player;
    WaitForSeconds wait;

    private void Awake()
    {
        player= GetComponent<Player>();
        wait = new WaitForSeconds(1f);
    }
    void Update()
    {
        blinkTimer      += Time.deltaTime;
        doubleShotTimer += Time.deltaTime;
        spade4Timer     += Time.deltaTime;
        homingTimer     += Time.deltaTime;
        if (GameManager.Instance.canFire) // 레벨업후 능력고를때 canfire = false 
        {
            if (!isBlock)
                Blink();

            DoubleShot();
            Spade4();
            Homing();
        }
    }


    //순간이동 스킬
    private void Blink()
    {

        if (Input.GetButtonDown("Jump") && blinkTimer > curblinkCool)
        {
            transform.Translate(GameManager.Instance.player.inputVec * Blinkrange);

            //잠시 속도상승 (스크립터블오브젝트로 햇다면 좀더 쉬웟을듯)
            if (blinkTempSpeed == 1)
                player.moveSpeed*=1.3f;
            else if (blinkTempSpeed == 2)
                player.moveSpeed *= 1.6f;

            StartCoroutine(BlinkTempMoveSpeedUp());
                blinkTimer = 0;
        }
    }
    IEnumerator BlinkTempMoveSpeedUp()
    {
        yield return wait;
        if (blinkTempSpeed == 1)
            player.moveSpeed /= 1.3f;
        else if (blinkTempSpeed == 2)
            player.moveSpeed /= 1.6f;
    }
    //더블샷
    private void DoubleShot()
    {
        if (Input.GetKeyDown(KeyCode.Q) && doubleShotTimer > curdoubleShotCool)
        {
            GameObject bulletL = GameManager.Instance.poolManager.Get(GameManager.Instance.poolManager.prefabs.Length - 1);
            bulletL.transform.position = transform.position;
            StartCoroutine(DoubleShotCoroutine());
            bulletL.GetComponent<Bullet>().Init(GameManager.Instance.damage* doubleShotDamageUp);
            doubleShotTimer = 0;
        }
    }
    //딜레이가 있어 겹쳐 날라가지않음
    IEnumerator DoubleShotCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject bulletR = GameManager.Instance.poolManager.Get(GameManager.Instance.poolManager.prefabs.Length - 1);
        bulletR.transform.position = transform.position;
        bulletR.GetComponent<Bullet>().Init(GameManager.Instance.damage * doubleShotDamageUp);
    }
    //스택이 있는적에 데미지를 주는 스킬(로아 아르카나같은 느낌)
    void Spade4()
    {
        if (Input.GetKeyDown(KeyCode.E) && spade4Timer > curSpade4Cool)
        {
            int zeroStackCheck=0;
            aroundenemys = Physics2D.CircleCastAll(transform.position, scanrange, Vector2.zero, 0, layerMask);

            //전부다 스택이 0이면 스킬을 쓸수없음
            foreach (RaycastHit2D item in aroundenemys)
            {
                if (item.transform.gameObject.GetComponent<Enemy>().stack != 0)
                    zeroStackCheck++;
            }

            if (zeroStackCheck==0)
                return;

            //스택이 있는 적에게 데미지가함
            foreach (RaycastHit2D item in aroundenemys)
            {
                float damage = item.transform.gameObject.GetComponent<Enemy>().stack *
                                            (GameManager.Instance.damage + (GameManager.Instance.damage * spade4ExtraDamage));
                item.transform.gameObject.GetComponent<Enemy>().health -= damage;
                if(damage>0)
                    DamageText.Create(item.transform.position, damage);

                item.transform.gameObject.GetComponent<Enemy>().Dead();
                item.transform.gameObject.GetComponent<Enemy>().stack = 0;
                item.transform.gameObject.GetComponent<Enemy>().UICardStack();
            }

            spade4Timer = 0;
        }

    }
    //유도탄
    void Homing()
    {
        if (Input.GetKeyDown(KeyCode.R) && canHoming && homingTimer > curhomingCool)
        {
            for (int i = 0; i < homingCount; i++)
            {
                GameObject bullet = GameManager.Instance.poolManager.Get(GameManager.Instance.poolManager.prefabs.Length - 1);
                bullet.transform.position = transform.position;
                Vector3 rotVec = Vector3.forward * 360 * i / homingCount;
                bullet.transform.Rotate(rotVec);
                bullet.GetComponent<Bullet>().Homing();
                homingTimer = 0;
            }
        }
    }



    //맵끝에 닿으면 blink못쓰게 하는 로직 // Ground오브젝트의 콜라이더 두께를 늘림
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isBlock = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isBlock = false;
        }
    }

}
