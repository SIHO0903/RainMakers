using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    //일반 공격
    void Update()
    {
        GameManager.Instance.fireRate -= Time.deltaTime;
        if (Input.GetButton("Fire1") && GameManager.Instance.fireRate <= 0 && GameManager.Instance.canFire)
        {
            GameObject bullet = GameManager.Instance.poolManager.Get(GameManager.Instance.poolManager.prefabs.Length - 1);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Init(0);
            GameManager.Instance.fireRate = GameManager.Instance.curFireRate;
            AudioManager.instance.SFXPlayer("Range");
        }

    }
}
