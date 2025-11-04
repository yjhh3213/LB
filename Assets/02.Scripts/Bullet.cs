using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 2.0f;
    
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
        
    }
}

