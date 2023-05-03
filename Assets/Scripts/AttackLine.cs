using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보스패턴중 대쉬 경로를 보여주는 스크립튼
public class AttackLine : MonoBehaviour
{
    [SerializeField] float lineSpeed;
    public Vector3 dashVec;
    WaitForSeconds wait;
    private void Awake()
    {
        wait = new WaitForSeconds(2f);
    }
    private void OnEnable()
    {
        //트레일렌더러의 위치 초기화
        transform.position = transform.parent.position;
        //대쉬 방향 설정
        dashVec = GameManager.Instance.player.transform.position - transform.position;
    }
    private void Update()
    {
        //트레일렌더러 대쉬경로가 생기는 속도
        transform.Translate(dashVec.normalized * lineSpeed * Time.deltaTime);
        StartCoroutine(SetFalse());
    }
    IEnumerator SetFalse()
    {
        yield return wait;
        gameObject.SetActive(false);
    }
}
