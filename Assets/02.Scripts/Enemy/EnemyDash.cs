using System.Collections;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    private Transform player;
    [Header("돌진 조건/파라미터")]
    public float triggerDistance = 8f;   // 이 거리 안에 들어오면 1회 돌진
    public float chargeSpeed = 18f;      // 돌진 속도
    public float chargeDuration = 0.6f;  // 돌진 유지 시간

    private bool isCharging = false;
    private bool hasCharged = false;     // 돌진은 딱 1번만
    SpriteRenderer spriteRenderer;
    Animator animator;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (player == null) return;

        // 아직 돌진 중이 아니면 기본 이동
        if (!isCharging && !hasCharged)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= triggerDistance)
            {
                StartCoroutine(ChargeOnce());
            }
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
        float diffx = player.position.x - transform.position.x;
        if (diffx > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (diffx < 0f)
        {
            spriteRenderer.flipX = true;
        }

        
    }

    IEnumerator ChargeOnce()
    {
        isCharging = true;
        hasCharged = true;   // 다시는 ChargeOnce 안 들어감, 대신 추적만 계속
        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }
        Vector3 dir = player != null ? (player.position - transform.position) : transform.right;
        if (dir.sqrMagnitude < 0.0001f) dir = transform.right;
        dir.Normalize();
        

        float t = 0f;
        while (t < chargeDuration)
        {
            transform.position += dir * chargeSpeed * Time.deltaTime;
            t += Time.deltaTime;    
            yield return null;
        }

        isCharging = false;
        
        if(animator != null)
        {
            animator.SetBool("Attack", false);
        }
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
