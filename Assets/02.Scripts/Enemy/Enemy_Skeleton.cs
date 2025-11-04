using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : MonoBehaviour
{
    public EnemyData data;
    public float EnemyHP;
    public Transform player;
    public float EnemySpeed;
    public float reviveDelay = 2f;

    enum State { Alive, Downed, Dead }
    State state = State.Alive;

    float hp;
    Vector3 downPos;
    Coroutine reviveCo;

    private void Start()
    {
        EnemyHP = data.hp;
        EnemySpeed = data.speed;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    private void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;

        transform.position += dir * EnemySpeed * Time.deltaTime;
    }


    public void TakeDamage(float damage)
    {
        if (state == State.Dead) return;
        if (state == State.Downed)
        {
            Die();
            return;
        }

        EnemyHP -= damage;
        if (EnemyHP < 0)
        {
            EnterDowned();
        }
    }
   
    void EnterDowned()
    {
        if (state != State.Alive) return;

        state = State.Downed;
        downPos = transform.position;

        if (reviveCo != null) StopCoroutine(reviveCo);
        reviveCo = StartCoroutine(CoReviveAfterDelay());
    }

    IEnumerator CoReviveAfterDelay()
    {
        yield return new WaitForSeconds(reviveDelay);

        if(state == State.Downed)
        {
            transform.position = downPos;
            hp = EnemyHP;
            state = State.Alive;
            reviveCo = null;
        }
    }

    void Die()
    {
        state = State.Dead;
        if(reviveCo != null) StopCoroutine(reviveCo);
        Destroy(gameObject);
    }
}
