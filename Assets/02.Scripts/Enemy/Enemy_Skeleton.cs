using System.Collections;
using UnityEngine;

public class Enemy_Skeleton : MonoBehaviour
{
    [Header("데이터")]
    public EnemyData data;

    [Header("스탯")]
    public float EnemyHP = 100f;      // 현재 HP
    public float EnemySpeed = 3f;     // 이동 속도

    [Header("참조")]
    public Transform player;

    [Header("다운/부활")]
    public float reviveDelay = 2f;    // 다운 유지 시간
    public float reviveHp = 30f;    // ★ 부활 시 HP(항상 양수로)
    public float reviveFreeze = 1.0f; // 부활 직후 가만히 있을 시간

    // 다운 직후 중복 히트 무시(그레이스)
    public float downedGrace = 0.05f; // 50ms
    float downedAt = -999f;

    enum State { Alive, Downed, Dead }
    State state = State.Alive;

    Vector3 downPos;
    Coroutine reviveCo;
    float freezeUntil = 0f;

    void Start()
    {
        if (data != null)
        {
            EnemyHP = data.hp;
            EnemySpeed = data.speed;
        }
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!player) return;

        // 다운/사망 또는 부활 직후 정지 중에는 이동 금지
        if (state != State.Alive) return;
        if (Time.time < freezeUntil) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * EnemySpeed * Time.deltaTime;
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
            transform.position = downPos;

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

        if (reviveCo != null) StopCoroutine(reviveCo);

        Debug.Log("[Skeleton] DEAD (destroy)");
        Destroy(gameObject);
    }
}
