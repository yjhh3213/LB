using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool hasHit = false;
    public float Damage = 2.0f;                     // 약점포착 level 0
    float DamageIn1 = (2.0f * 0.2f) + 2.0f;         // 약점포착 level 1
    float DamageIn2 = (2.0f * 0.4f) + 2.0f;         // 약점포착 level 2
    float DamageIn3 = (2.0f * 0.5f) + 2.0f;         // 약점포착 level 3
    int BulletCardLevel;                            // 총알개조 level

    BoxCollider2D bc;
    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        BulletCardLevel = Card.Instance.BulletCard;

        int weaknessCardLevel = Card.Instance.weaknessCard;
        if (weaknessCardLevel == 0) Damage = 2.0f;
        else if (weaknessCardLevel == 1) Damage = DamageIn1;
        else if (weaknessCardLevel == 2) Damage = DamageIn2;
        else if (weaknessCardLevel >= 3) Damage = DamageIn3;

        if (BulletCardLevel > 0) bc.isTrigger = true;
        else bc.isTrigger = false;
    }

    int count = 0;                                    // 관통한 횟수 초기화

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Skeleton")) return;

        EnemyStat enemy = collision.GetComponent<EnemyStat>();
        Enemy_Skeleton enemy_Skeleton = collision.GetComponent<Enemy_Skeleton>();
        Enemy_Boar enemy_Boar = collision.GetComponent<Enemy_Boar>();
        if (enemy != null)
        {
            if (enemy.isDead) return;
            enemy.TakeDamage(Damage);
        }
        if(enemy_Skeleton != null)
        {   
            enemy_Skeleton.TakeDamage(Damage);
        }
        if(enemy_Boar != null)
        {   
            if(enemy.isDead) return;
            enemy_Boar.TakeDamage(Damage);
        }

        BCL(BulletCardLevel);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy") && !collision.collider.CompareTag("Skeleton")) return;

        EnemyStat enemy = collision.collider.GetComponent<EnemyStat>();
        Enemy_Skeleton enemy_Skeleton= collision.collider.GetComponent<Enemy_Skeleton>();
        Enemy_Boar enemy_Boar = collision.collider.GetComponent <Enemy_Boar>();
        if (enemy != null)
            enemy.TakeDamage(Damage);
        if(enemy_Skeleton != null)
        {
            enemy_Skeleton.TakeDamage(Damage);
        }
        if(enemy_Boar != null)
        {
            enemy_Boar.TakeDamage(Damage);
        }

        Destroy(gameObject); // 관통 없으면 즉시 파괴
    }
    
    // 총알개조 Level에 따른 관통할 수 있는 코드
    // 적을 추가할 때마다 메서드를 추가하면 됨

    void BCL(int level)
    {
        if (BulletCardLevel == 1)
        {
            if (count == 1)
            {
                count = 0;
                Destroy(gameObject);
            }
            count++;
        }
        else if (BulletCardLevel == 2)
        {
            if (count == 2)
            {
                count = 0;
                Destroy(gameObject);
            }
            count++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}