using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField]float speed;
    public float damage;
    public bool canHoming;
    Rigidbody2D rigid;
    WaitForSeconds homing;
    WaitForSeconds activeFalse;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        homing = new WaitForSeconds(0.5f);
        activeFalse = new WaitForSeconds(4f);
    }
    private void FixedUpdate()
    {
        if(canHoming)
        {
            StartCoroutine(HomingDelay());
        }
    }
    
    public void Init(float extraDamage)
    {
        //총알이 날라가는 방향
        Vector2 dirVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                         GameManager.Instance.player.transform.position;
        //총알이 보는 방향
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
        //총알 속도
        rigid.velocity = dirVec.normalized * speed;

        damage = GameManager.Instance.damage + extraDamage;
        StartCoroutine(SetFalse());
    }
    //호밍전 플레이어기준 원모양으로 총알 발사
    public void Homing()
    {
        damage = GameManager.Instance.damage;
        rigid.velocity = transform.up * speed;
        canHoming= true;
        StartCoroutine(SetFalse());
    }

    //호밍 시작
    IEnumerator HomingDelay()
    {
        yield return homing;
        Vector2 direction = GameManager.Instance.player.scanner.TargetNearest().position - transform.position; //날라가는 방향
        direction.Normalize();
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction); //보는 방향
        
        //총알이 위로만 날라가도록
        float rotateAmount = Vector3.Cross(direction, transform.up).z; 
        rigid.angularVelocity = -rotateAmount * 300f;
        rigid.velocity = direction* speed;
    }

    //비활성화하면서 호밍 초기화
    IEnumerator SetFalse()
    {  
        yield return activeFalse;
        canHoming = false;
        gameObject.SetActive(false);
 
    }
    //충돌시 비활성화
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            canHoming = false;
            gameObject.SetActive(false);
        }
    }
}
