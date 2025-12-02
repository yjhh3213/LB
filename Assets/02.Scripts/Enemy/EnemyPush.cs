using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPush : MonoBehaviour
{
    public float PushRadius = 0.7f;         // 거리가 PushRadius일때
    public float PushForce = 3.0f;          // PushForce만큼 밀어내기
    EnemyStat ES;
    // Start is called before the first frame update
    private void Start()
    {
        ES = GetComponent<EnemyStat>();
    }

    private void FixedUpdate()
    {
        Push();
    }

    void Push()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, PushRadius);

        foreach(var hit in hits)
        {
            if(!ES.isDead)
            {
                if (hit.gameObject == gameObject) continue;
                if (!hit.CompareTag("Enemy") && !hit.CompareTag("Skeleton") && !hit.CompareTag("Boar")) continue;

                // 방향 계산
                Vector2 dir = (transform.position - hit.transform.position).normalized;

                // 밀어내기
                transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
            }
        }
    }
}
