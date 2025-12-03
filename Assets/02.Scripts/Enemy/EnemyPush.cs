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
            if (hit.gameObject == gameObject) continue;
            if (ES != null && !ES.isDead && hit.CompareTag("Enemy"))
            {
                Vector2 dir = (transform.position - hit.transform.position).normalized;
                transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
            }

            if (E_SK != null && !E_SK.isDead && hit.CompareTag("Skeleton"))
            {
                Vector2 dir = (transform.position - hit.transform.position).normalized;
                transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
            }

            if (E_B != null && !E_B.isDead && hit.CompareTag("Boar"))
            {
                Vector2 dir = (transform.position - hit.transform.position).normalized;
                transform.position += (Vector3)(dir * PushForce * Time.deltaTime);
            }
        }
    }
}
