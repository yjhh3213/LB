using System.Collections;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    public Transform player;

    [Header("기본 이동")]
    public float moveSpeed = 3f;

    [Header("돌진 조건/파라미터")]
    public float triggerDistance = 8f;   // 이 거리 안에 들어오면 1회 돌진
    public float chargeSpeed = 18f;      // 돌진 속도
    public float chargeDuration = 0.6f;  // 돌진 유지 시간

    private bool isCharging = false;
    private bool hasCharged = false;     // 돌진은 딱 1번만

    void Update()
    {
        if (player == null) return;

        // 아직 돌진 중이 아니면 기본 이동
        if (!isCharging)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }

        // 트리거: 아직 돌진 안 했고, 거리 조건 만족 시 1회 돌진 시작
        if (!hasCharged && !isCharging)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= triggerDistance)
            {
                StartCoroutine(ChargeOnce());
            }
        }
    }

    IEnumerator ChargeOnce()
    {
        isCharging = true;

        // 돌진 시작 순간의 방향을 고정 (플레이어가 도중에 움직여도 방향은 변하지 않음)
        Vector3 dir = player != null ? (player.position - transform.position) : transform.forward;
        if (dir.sqrMagnitude < 0.0001f) dir = transform.forward;
        dir.Normalize();

        float t = 0f;
        while (t < chargeDuration)
        {
            transform.position += dir * chargeSpeed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        hasCharged = true;   // 재돌진 불가
        isCharging = false;
    }

#if UNITY_EDITOR
    // 에디터에서 트리거 거리 확인용(선택)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
#endif
}
