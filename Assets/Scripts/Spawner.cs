using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 스폰
//리스크오브레인2처럼 시간에 따라 가중치를 둬서 생성되는 유닛종류, 유닛스탯증가, 엘리트몹을 만들면 좋을듯
public class Spawner : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    //플레이어 주변, 맵밖으로 포인트를 잡음
    [SerializeField] Transform[] spawnpoint;
    [SerializeField] float bossSpawnTime; //보스가 소환되는 시간
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
        //보스소환 로직
        if (GameManager.Instance.gameTime > bossSpawnTime && !isBoss)
        {
            BossSpawn();
            isBoss = true;
            isSpawning= false;
        }
        else if(isSpawning)
        {
            spawntimer += Time.deltaTime;
            if(spawntimer > 1f ) //1초마다 스폰
            {
                Spawn();
                levelByTime++;
                spawntimer = 0;
            }
        }
    }

    //일반 몹 소환
    void Spawn()
    {
        GameObject enemy = poolManager.Get(Random.Range(0, Mathf.Min(levelByTime/3,5))); // 1*3초마다 스폰되는 적종류 증가
        enemy.transform.position = spawnpoint[Random.Range(1,spawnpoint.Length)].position;
    }
    //보스소환
    void BossSpawn()
    {
        GameObject boss = poolManager.Get(6);
        boss.transform.position = spawnpoint[Random.Range(1, spawnpoint.Length)].position;
        AudioManager.instance.BGMBoss();

    }
}
