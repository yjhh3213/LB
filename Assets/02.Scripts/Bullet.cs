using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 2.0f;                     // 약점포착 level 0
    float DamageIn1 = (2.0f * 0.2f) + 2.0f;         // 약점포착 level 1
    float DamageIn2 = (2.0f * 0.4f) + 2.0f;         // 약점포착 level 2
    float DamageIn3 = (2.0f * 0.5f) + 2.0f;         // 약점포착 level 3

    private void Start()
    {
        int weaknessCardLevel = Card.Instance.weaknessCard;
        if (weaknessCardLevel == 0) Damage = 2.0f;
        else if (weaknessCardLevel == 1) Damage = DamageIn1;
        else if (weaknessCardLevel == 2) Damage = DamageIn2;
        else if (weaknessCardLevel >= 3) Damage = DamageIn3;

        //print(Damage);
    }
    int count = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyStat enemy = collision.collider.GetComponent<EnemyStat>();
            if (enemy != null )
            {
                enemy.TakeDamage(Damage);
            }

            if (collision.collider.CompareTag("aa"))
            {
                if (hasHit) return;
                Enemy_Skeleton enemy_Skeleton = collision.collider.GetComponent<Enemy_Skeleton>();
                if (enemy_Skeleton != null)
                {
                    enemy_Skeleton.TakeDamage(Damage);
                }
            }

            // bullet이 프리팹 상태이기에 Card 스크립트를 Instance화를 시켜 해당하는 값을 가져오기
            int BulletCardLevel = Card.Instance.BulletCard;

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
                if(count == 2)
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
}
