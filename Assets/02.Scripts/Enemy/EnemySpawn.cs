using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instance;
    [Header("몬스터 설정")]
    public GameObject monsterPrefabA; // 1~8라 전부 (기본좀비)
    public GameObject monsterPrefabB; // 3~8라 (좀비 멧돼지)
    public GameObject monsterPrefabC; // 4~8라 (돌연변이)
    public GameObject monsterPrefabD; // 2~8라 (스켈레톤)
    public GameObject monsterPrefabE; // 3 ~8라 (베놈)

    [Header("몬스터 스폰시간")]
    public float spawnDelay = 0.2f; // 스폰 시간 조절

    [Header("웨이브 설정")]
    public int minWave = 1;  // 최소 웨이브
    public int maxWave = 8; // 최대 웨이브

    [Header("기본좀비 규칙")]
    public int A_startCount = 20;   // 1웨이브 기본 수
    public int A_addPerWave = 10;   // 웨이브마다 추가

    [Header("좀비 멧돼지 규칙")]
    public int B_startWave = 3;   // 시작 웨이브
    public int B_startCount = 5; //  첫 등장 수
    public int B_addper2Wave = 2; // 짝수 웨이브 마다 누적 

    [Header("돌연변이 몬스터 규칙")]
    public int C_startWave = 4; // 시작 웨이브
    public int C_startCount = 5; // 첫 등장수
    public int C_addperWave = 1; // 웨이브 추가

    [Header("스켈레톤 좀비 규칙")]
    public int D_startWave = 2; // 시작 웨이브
    public int D_startCount = 5; // 첫 등장수
    public int D_addPerWave = 2; // 웨이브 추가

    [Header("베놈 좀비 규칙")]
    public int E_startWave = 3; // 시작 웨이브
    public int E_startCount = 1; // 첫 등장수
    public int E_addPerWave = 1; // 웨이브 추가

    [Header("맵 범위")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;
    public float minY = -50f;
    public float maxY = 50f;

    [Header("참조")]
    public CountTimer countTimer;

    private bool spawning = false;

    [Header("현재 필드 몬스터 수")]
    public int FiledEnemy = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;   // 🔥 EnemySpawn.Instance 로 접근 가능해짐
        }
        //else
        //{
        //    Destroy(gameObject); // 중복 EnemySpawn 제거
        //}
    }
    void Update()
    {
        if (countTimer == null) return;

        int wave = countTimer.CurrentWave;

        if (wave < minWave || wave > maxWave) return;
        if (maxWave < countTimer.CurrentWave) return;
        // WaveEnded가 true이면 새로운 웨이브 시작
        if (countTimer.WaveEnded && !spawning)
        {
            StartCoroutine(SpawnWave());
            countTimer.ResteWaveFlag(); // 플래그 초기화
        }
    }

    IEnumerator SpawnWave()
    {
        spawning = true;

        int wave = countTimer.CurrentWave;
        int aCount = A_startCount + (wave - 1) * A_addPerWave;
        int bCount = 0;
        int cCount = 0;
        int dCount = 0;
        int eCount = 0;

        if (wave >= B_startWave && wave <= maxWave)
        {
            bCount = B_startCount;

            int evenSteps = (wave / 2) - ((B_startWave - 1) / 2);
            if (evenSteps > 0)
            {
                bCount += evenSteps * B_addper2Wave;
            }
        }

        if (wave >= C_startWave && wave <= maxWave)
        {
            cCount = C_startCount + (wave - 4) * C_addperWave;
        }

        if (wave >= D_startWave && wave <= maxWave)
        {
            dCount = D_startCount + (wave - 2) * D_addPerWave;
        }

        if(wave >= E_startWave && wave <= maxWave)
        {
            eCount = E_startCount + (wave - 3) * E_addPerWave;
        }
        Debug.Log($"[Wave {wave}] Spawn A: {aCount}, B: {bCount} C : {cCount} D : {dCount} E : {eCount}");

        List<GameObject> toSpawn = new List<GameObject>();
        for (int i = 0; i < aCount; i++) toSpawn.Add(monsterPrefabA);
        for (int i = 0; i < bCount; i++) toSpawn.Add(monsterPrefabB);
        for (int i = 0; i < cCount; i++) toSpawn.Add(monsterPrefabC);
        for (int i = 0; i < dCount; i++) toSpawn.Add(monsterPrefabD);
        for (int i = 0; i < eCount; i++) toSpawn.Add(monsterPrefabE);

        // ===== 리스트 섞기 (Fisher–Yates) =====
        for (int i = 0; i < toSpawn.Count; i++)
        {
            int rand = Random.Range(i, toSpawn.Count);
            GameObject temp = toSpawn[i];
            toSpawn[i] = toSpawn[rand];
            toSpawn[rand] = temp;
        }

        // ===== 섞은 순서대로 스폰 =====
        foreach (var prefab in toSpawn) // 타입 변수명 in 컬렉션 명 
        {
            SpawnMonster(prefab);
            if (spawnDelay > 0f)
                yield return new WaitForSeconds(spawnDelay);
            //else
            //    yield return null;
        }

        spawning = false;
    }

    void SpawnMonster(GameObject prefab)
    {
        if (prefab == null) return;
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float randomY = Random.Range(minY, maxY);
        Instantiate(prefab, new Vector3(randomX, randomY, randomZ), Quaternion.identity);

        FiledEnemy++;
    }
    public void OnEnemyDied()
    {
        FiledEnemy--;

        if (FiledEnemy <= 0)
        {
            Debug.Log("필드 몬스터 전부 사망 → 웨이브 종료");
            countTimer.EndWaveByEnemies();
        }


    }
}


