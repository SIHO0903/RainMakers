using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������ �ִ��� Ÿ����
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
        //�������� ��Ÿ��ȿ� ���� ������ enemys�� ����
        enemys = Physics2D.CircleCastAll(transform.position, scanrange, Vector2.zero, 0, layerMask);

        
        foreach (RaycastHit2D item in enemys)
        {
            
            Vector2 disVec = item.transform.position - transform.position;
            if (disVec.magnitude <= distanceCheck) //��ó�� ���߰� �� Ÿ������
            {
                transform.parent.GetComponent<Skill>().canHoming = true;
                nearest = item.transform;
                distanceCheck = disVec.magnitude;
            }
            if (nearest != null && !nearest.gameObject.activeSelf) //�� ��Ȱ��ȭ��
            {
                transform.parent.GetComponent<Skill>().canHoming = false;
                distanceCheck = 10;
            }

        }
        return nearest;
    }

}
