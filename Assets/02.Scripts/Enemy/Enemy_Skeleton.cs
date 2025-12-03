using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Enemy_Skeleton : MonoBehaviour
{
    [Header("데이터")]
    public EnemyData data;

    [Header("스탯")]
    public float EnemyHP = 1f;      // 현재 HP
    public float EnemySpeed = 1f;     // 이동 속도

    [Header("참조")]
    private Transform player;

    [Header("다운/부활")]
    public float reviveDelay = 3f;    // 다운 유지 시간
    public float reviveHp = 30f;    // ★ 부활 시 HP(항상 양수로)
    public float reviveFreeze = 1.0f; // 부활 직후 가만히 있을 시간

    // 다운 직후 중복 히트 무시(그레이스)
    public float downedGrace = 0.05f; // 50ms
    float downedAt = -999f;

    public enum State { Alive, Downed, Dead }
    public State state = State.Alive;

    Vector3 downPos;
    Coroutine reviveCo;
    float freezeUntil = 0f;

    private SpriteRenderer HandspriteRenderer;
    private SpriteRenderer FeetspriteRenderer;
    SpriteRenderer spriteRenderer;
    public GameObject Hand; //손
    public GameObject Feet; //발
    public GameObject tlcp; //시체
    public Sprite[] r_sprites; //랜덤으로 금간 모습
    public Sprite[] headOnlySprites; //머리만 남은 해골 스프라이트 배열
    public Sprite[] skeletonDebrisSprites;
    public int foseja; //랜덤 결정
    public float dieAnimTime = 1.0f;

    private Sprite originalSprite; // 원래 스프라이트 저장
    private List<GameObject> activeDebris = new List<GameObject>(); // 생성된 파편 추적

    public bool isDead = false;

    void Start()
    {
        foseja = Random.Range(0, r_sprites.Length);
        if (data != null)
        {
            EnemyHP = data.hp;
            EnemySpeed = data.speed;
        }
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            HandspriteRenderer = Hand.GetComponentInChildren<SpriteRenderer>();
            FeetspriteRenderer = Feet.GetComponentInChildren<SpriteRenderer>();
            originalSprite = r_sprites[foseja];
            spriteRenderer.sprite = originalSprite;
        }
    }

    void Update()
    {
        if (!player) return;

        // 다운/사망 또는 부활 직후 정지 중에는 이동 금지
        if (state != State.Alive) return;
        if (Time.time < freezeUntil) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * EnemySpeed * Time.deltaTime;

        float diffx = player.position.x - transform.position.x;

        Vector3 handPosition = transform.position;

        if (diffx > 0f) // 플레이어가 오른쪽에 있을 때 (몬스터가 오른쪽을 바라볼 때)
        {
            spriteRenderer.flipX = false;

            handPosition.x += 0.25f;
            handPosition.y -= 0.2f;
            Hand.transform.position = handPosition;

            HandspriteRenderer.flipY = false; // 90도 돌아가있음
            FeetspriteRenderer.flipX = false;
        }
        else if (diffx < 0f) // 플레이어가 왼쪽에 있을 때 (몬스터가 왼쪽을 바라볼 때)
        {
            spriteRenderer.flipX = true;

            handPosition.x -= 0.25f;
            handPosition.y -= 0.2f;
            Hand.transform.position = handPosition;

            HandspriteRenderer.flipY = true; // 90도 돌아가있음
            FeetspriteRenderer.flipX = true;
        }
    }

    // 총알/무기에서 이 둘 중 하나 호출해도 됨
    public void TakeDamage(float dmg) => ApplyDamageInternal(dmg);

    void ApplyDamageInternal(float dmg)
    {
        if (state == State.Dead) return;

        // ★ 다운 직후 그레이스: 같은 프레임 연속 히트 무시
        if (state == State.Downed && Time.time - downedAt < downedGrace) return;

        // 다운 상태에서 맞으면 '진짜 사망'(유일한 사망 조건)
        if (state == State.Downed)
        {
            Die();
            return;
        }

        // 평소엔 HP 감소 → 0에 도달하면 다운
        float prev = EnemyHP;
        EnemyHP = Mathf.Max(0f, EnemyHP - dmg);
        if (prev > 0f && EnemyHP == 0f)  // "0에 도달한 순간"만 다운
        {
            EnterDowned();
        }
    }

    void EnterDowned()
    {
        if (state != State.Alive) return;

        state = State.Downed;
        downPos = transform.position;
        downedAt = Time.time;  // ★ 그레이스 시작

        // 손발 비활성화
        Hand.SetActive(false);
        Feet.SetActive(false);

        // 머리만 남은 스프라이트로 변경
        if (headOnlySprites != null && headOnlySprites.Length > 0)
        {
            spriteRenderer.sprite = headOnlySprites[foseja];
        }

        // 파편 생성
        activeDebris.Clear();
        for (int i = 0; i < skeletonDebrisSprites.Length; i++)
        {
            GameObject debris = Instantiate(tlcp, transform.position, Quaternion.identity);
            Enemy_Skeleton_tlcp debrisScript = debris.GetComponent<Enemy_Skeleton_tlcp>();
            debrisScript.qhscp = gameObject;
            debrisScript.SetDebrisSprites(skeletonDebrisSprites);
            debrisScript.SetSpriteIndex(i);

            activeDebris.Add(debris); // 파편 추적
        }

        Debug.Log($"[Skeleton] DOWNED at {downPos} (revive in {reviveDelay}s)");

        if (reviveCo != null) StopCoroutine(reviveCo);
        reviveCo = StartCoroutine(CoReviveAfterDelay());
    }

    IEnumerator CoReviveAfterDelay()
    {
        yield return new WaitForSeconds(reviveDelay);

        // 다운 유지 중이라면 부활(마무리 타격을 못 받은 경우)
        if (state == State.Downed)
        {
            // 파편 제거
            foreach (GameObject debris in activeDebris)
            {
                if (debris != null)
                    Destroy(debris);
            }
            activeDebris.Clear();

            // 스프라이트 복구
            spriteRenderer.sprite = originalSprite;

            // 손발 활성화
            Hand.SetActive(true);
            Feet.SetActive(true);

            // ★ 부활 HP 확실히 양수로
            EnemyHP = Mathf.Max(1f, reviveHp);

            state = State.Alive;
            reviveCo = null;

            // ★ 부활 직후 잠깐 멈춤
            freezeUntil = Time.time + reviveFreeze;

            Debug.Log($"[Skeleton] REVIVED at {downPos} with HP={EnemyHP} (freeze {reviveFreeze}s)");
        }
    }

    void Die()
    {
        if (state == State.Dead) return;
        state = State.Dead;

        if (isDead) return; // 두 번 실행 방지
        isDead = true;

        if (reviveCo != null) StopCoroutine(reviveCo);
        if (EnemySpawn.Instance != null) EnemySpawn.Instance.OnEnemyDied();
        // 모이는 중인 파편들 정지 후 중력으로 떨어뜨리기
        foreach (GameObject debris in activeDebris)
        {
            if (debris != null)
            {
                Enemy_Skeleton_tlcp debrisScript = debris.GetComponent<Enemy_Skeleton_tlcp>();
                if (debrisScript != null)
                {
                    debrisScript.StopAndFall();
                }
            }
        }
        activeDebris.Clear();

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!isDead) return; // 이미 Die() 처리되었으면 무시

        //if (EnemySpawn.Instance != null)
        //    EnemySpawn.Instance.FiledEnemy = Mathf.Max(EnemySpawn.Instance.FiledEnemy - 1, 0);
    }
}