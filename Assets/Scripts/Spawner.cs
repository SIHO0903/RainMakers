using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����
//����ũ���극��2ó�� �ð��� ���� ����ġ�� �ּ� �����Ǵ� ��������, ���ֽ�������, ����Ʈ���� ����� ������
public class Spawner : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    //�÷��̾� �ֺ�, �ʹ����� ����Ʈ�� ����
    [SerializeField] Transform[] spawnpoint;
    [SerializeField] float bossSpawnTime; //������ ��ȯ�Ǵ� �ð�
    float spawntimer;
    int levelByTime;

    bool isBoss;
    public bool isSpawning =true;
    void Awake()
    {
        spawnpoint = GetComponentsInChildren<Transform>();
    }
    private void OnEnable()
    {
        spawntimer = 0;
        levelByTime = 0;
        isSpawning = true;
    }
    void Update()
    {
        //������ȯ ����
        if (GameManager.Instance.gameTime > bossSpawnTime && !isBoss)
        {
            BossSpawn();
            isBoss = true;
            isSpawning= false;
        }
        else if(isSpawning)
        {
            spawntimer += Time.deltaTime;
            if(spawntimer > 1f ) //1�ʸ��� ����
            {
                Spawn();
                levelByTime++;
                spawntimer = 0;
            }
        }
    }

    //�Ϲ� �� ��ȯ
    void Spawn()
    {
        GameObject enemy = poolManager.Get(Random.Range(0, Mathf.Min(levelByTime/3,5))); // 1*3�ʸ��� �����Ǵ� ������ ����
        enemy.transform.position = spawnpoint[Random.Range(1,spawnpoint.Length)].position;
    }
    //������ȯ
    void BossSpawn()
    {
        GameObject boss = poolManager.Get(6);
        boss.transform.position = spawnpoint[Random.Range(1, spawnpoint.Length)].position;
        AudioManager.instance.BGMBoss();

    }
}
