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
        if (GameManager.Instance.canFire) // �������� �ɷ°��� canfire = false 
        {
            if (!isBlock)
                Blink();

            DoubleShot();
            Spade4();
            Homing();
        }
    }


    //�����̵� ��ų
    private void Blink()
    {

        if (Input.GetButtonDown("Jump") && blinkTimer > curblinkCool)
        {
            transform.Translate(GameManager.Instance.player.inputVec * Blinkrange);

            //��� �ӵ���� (��ũ���ͺ������Ʈ�� �޴ٸ� ���� ���m����)
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
    //����
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
    //�����̰� �־� ���� ����������
    IEnumerator DoubleShotCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject bulletR = GameManager.Instance.poolManager.Get(GameManager.Instance.poolManager.prefabs.Length - 1);
        bulletR.transform.position = transform.position;
        bulletR.GetComponent<Bullet>().Init(GameManager.Instance.damage * doubleShotDamageUp);
    }
    //������ �ִ����� �������� �ִ� ��ų(�ξ� �Ƹ�ī������ ����)
    void Spade4()
    {
        if (Input.GetKeyDown(KeyCode.E) && spade4Timer > curSpade4Cool)
        {
            int zeroStackCheck=0;
            aroundenemys = Physics2D.CircleCastAll(transform.position, scanrange, Vector2.zero, 0, layerMask);

            //���δ� ������ 0�̸� ��ų�� ��������
            foreach (RaycastHit2D item in aroundenemys)
            {
                if (item.transform.gameObject.GetComponent<Enemy>().stack != 0)
                    zeroStackCheck++;
            }

            if (zeroStackCheck==0)
                return;

            //������ �ִ� ������ ����������
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
    //����ź
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



    //�ʳ��� ������ blink������ �ϴ� ���� // Ground������Ʈ�� �ݶ��̴� �β��� �ø�
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
