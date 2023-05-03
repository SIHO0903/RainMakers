using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//가장 가까이 있는적 타겟팅
public class Scanner : MonoBehaviour
{
    RaycastHit2D[] enemys;
    [Header("# NearestTargetSearch")]
    public Transform nearest;
    [SerializeField] float scanrange;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float distanceCheck;


    void Update()
    {
        TargetNearest();
    }

    public Transform TargetNearest()
    {
        //원형태의 사거리안에 적이 들어오면 enemys에 저장
        enemys = Physics2D.CircleCastAll(transform.position, scanrange, Vector2.zero, 0, layerMask);

        
        foreach (RaycastHit2D item in enemys)
        {
            
            Vector2 disVec = item.transform.position - transform.position;
            if (disVec.magnitude <= distanceCheck) //근처에 적발견 및 타겟지정
            {
                transform.parent.GetComponent<Skill>().canHoming = true;
                nearest = item.transform;
                distanceCheck = disVec.magnitude;
            }
            if (nearest != null && !nearest.gameObject.activeSelf) //적 비활성화됨
            {
                transform.parent.GetComponent<Skill>().canHoming = false;
                distanceCheck = 10;
            }

        }
        return nearest;
    }

}
