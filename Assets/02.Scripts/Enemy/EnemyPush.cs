using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPush : MonoBehaviour
{
    public float PushRadius = 0.7f;         // 거리가 PushRadius일때
    public float PushForce = 3.0f;          // PushForce만큼 밀어내기
    EnemyStat ES;
    Enemy_Skeleton E_SK;
    Enemy_Boar E_B;
    // Start is called before the first frame update
    private void Start()
    {
        ES = GetComponent<EnemyStat>();
        E_SK = GetComponent<Enemy_Skeleton>();
        E_B = GetComponent<Enemy_Boar>();
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
            if (ES || E_SK || E_B == null) return;
            if(ES || E_B || E_SK != null)
            {
                if (!ES.isDead)
                {
                    if (hit.gameObject == gameObject) continue;
                    if (!hit.CompareTag("Enemy")) continue;

                    // 방향 계산
                    Vector2 dir = (transform.position - hit.transform.position).normalized;

                    // 밀어내기
                    transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
                }

                if (!E_SK.isDead)
                {
                    if (hit.gameObject == gameObject) continue;
                    if (!hit.CompareTag("Skeleton")) continue;

                    // 방향 계산
                    Vector2 dir = (transform.position - hit.transform.position).normalized;

                    // 밀어내기
                    transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
                }

                if (!E_B.isDead)
                {
                    if (hit.gameObject == gameObject) continue;
                    if (!hit.CompareTag("Boar")) continue;

                    // 방향 계산
                    Vector2 dir = (transform.position - hit.transform.position).normalized;

                    // 밀어내기
                    transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
                }
            }
        }
    }
}
