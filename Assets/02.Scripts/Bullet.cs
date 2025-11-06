using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 2.0f;
    bool hasHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            EnemyStat enemy = collision.collider.GetComponent<EnemyStat>();
            if (enemy != null )
            {
                enemy.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
        
        if(collision.collider.CompareTag("aa"))
        {
            if (hasHit) return;
            Enemy_Skeleton enemy_Skeleton = collision.collider.GetComponent<Enemy_Skeleton>();
            if(enemy_Skeleton != null)
            {
                enemy_Skeleton.TakeDamage(Damage);
            }
        }
    }

}

