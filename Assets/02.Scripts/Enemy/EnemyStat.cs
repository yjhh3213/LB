using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStat : MonoBehaviour
{
    public EnemyData data;
    private Transform player;
    public float EnemySpeed;
    public float EnemyHP;
    bool isDead = false;
    public SpriteRenderer spriteRenderer;
    private void Start() {
        if (data == null)
        {
            Debug.LogWarning("몬스터 데이터가 연결되지 않았습니다");
            return;
        }
        else if(data != null)
        {
            EnemySpeed = data.speed;
            EnemyHP = data.hp;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if(player == null)
        {
            Debug.LogWarning("Player가 연결되지 않았습니다");
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void Update() {
        if(player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;

        transform.position += dir * EnemySpeed * Time.deltaTime;

        // 몬스터가 플레이어 방향 바라보게 하는 코드 
        float diffx = player.position.x - transform.position.x;
        if(diffx > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if(diffx < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage(float damage)
    {
        print(damage);
        EnemyHP -= damage;
        print("Enemy HP : " + EnemyHP);

        if (EnemyHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // 두 번 실행 방지
        isDead = true;
        if (EnemySpawn.Instance != null)
        {
            EnemySpawn.Instance.OnEnemyDied();
        }
        Destroy(gameObject);
    }
}
