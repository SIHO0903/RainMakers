using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//������Ʈ Ǯ��
public class PoolManager : MonoBehaviour
{
    //������ ����
    public GameObject[] prefabs;
    List<GameObject>[] pools;
    private void Awake()
    {
        //�������� ���̿� ���� List����
        pools = new List<GameObject>[prefabs.Length];

        //List �ʱ�ȭ
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        //index�ش��ϴ� ������ ��Ȱ��ȭ�� Ȱ��ȭ�� ��ȯ
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // �������� ���� Ȱ��ȭ�����Ͻ� ���� ������ ����Ʈ�� �߰�
        if (!select)
        {

            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
