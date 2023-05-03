using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �Ѿ�
public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float speed;
    public float rotateSpeed;
    public float damage;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //ȣ��
        Vector3 direction = GameManager.Instance.player.transform.position - transform.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        rigid.angularVelocity = rotateAmount * rotateSpeed;
        rigid.velocity = transform.right * speed;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            gameObject.SetActive(false);
    }
}
