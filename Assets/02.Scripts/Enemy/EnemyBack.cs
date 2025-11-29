using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBack : MonoBehaviour
{
    private float knockbackSpeed = 20f;   // 밀리는 속도 (조절값)
    private float knockbackTime = 0.1f;  // 넉백 유지 시간

    private Rigidbody2D rb;
    private bool isKnockback = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;  // 빙글빙글 방지
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            StartCoroutine(KnockbackRoutine(dir));
        }
    }

    System.Collections.IEnumerator KnockbackRoutine(Vector2 dir)
    {
        if (isKnockback) yield break;
        isKnockback = true;

        float timer = 0f;

        // 넉백 시간 동안 일정 속도로 밀리기
        while (timer < knockbackTime)
        {
            rb.velocity = dir * knockbackSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        // 넉백 종료 후 멈춤
        rb.velocity = Vector2.zero;

        isKnockback = false;
    }

    public void StopKnockback()
    {
        isKnockback = false;
        StopAllCoroutines();
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
}