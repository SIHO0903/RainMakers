using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//오브젝트 풀링
public class PoolManager : MonoBehaviour
{
    //프리팹 저장
    public GameObject[] prefabs;
    List<GameObject>[] pools;
    private void Awake()
    {
        //프리팹의 길이에 따라 List선언
        pools = new List<GameObject>[prefabs.Length];

        //List 초기화
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        //index해당하는 프리팹 비활성화시 활성화로 전환
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 프리팹이 전부 활성화상태일시 새로 생성후 리스트에 추가
        if (!select)
        {

            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
