using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public EnemyData data;
    private Transform player;
    public float EnemySpeed;
    public float EnemyHP;
    bool isDead = false;

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
    }

    private void Update() {
        if(player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;

        transform.position += dir * EnemySpeed * Time.deltaTime;

        
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
