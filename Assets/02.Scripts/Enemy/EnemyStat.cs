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
    SpriteRenderer spriteRenderer;
    Animator anim;
    public float dieAnimTime = 0.7f;
    public GameObject poisonCloundPrefab;
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

        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator를 찾지 못했습니다! Enemy 프리팹에 Animator 컴포넌트가 있는지 확인하세요.");
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
        EffectManager.Instance.PlayAnimation("피격피", transform.position, 1f, 0.25f, 0.1f); // 이펙트 생성
        EnemyHP -= damage;
        print("Enemy HP : " + EnemyHP);

        if (EnemyHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        if (EnemySpawn.Instance != null)
        {
            EnemySpawn.Instance.FiledEnemy = Mathf.Max(EnemySpawn.Instance.FiledEnemy - 1, 0);
        }


        if (isDead) return; // 두 번 실행 방지
        isDead = true;

        

        EnemyBack back = GetComponent<EnemyBack>(); // 뒤로 밀리다가 죽으면 멈추게 
        if (back != null)
            back.StopKnockback();
        EnemySpeed = 0;
        if(poisonCloundPrefab != null)
        {
            Instantiate(poisonCloundPrefab, transform.position, Quaternion.identity);
        }
        if (anim != null)
        {
            anim.SetBool("Die" , true);
            
        }

        if (EnemySpawn.Instance != null)
        {
            EnemySpawn.Instance.OnEnemyDied();
        }

        // 🔥 GameManager KillCount 증가
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
            gm.killCount++;

        StartCoroutine(DieDestroyCoroutine());

        IEnumerator DieDestroyCoroutine()
        {
            yield return new WaitForSeconds(dieAnimTime);
            Destroy(gameObject);
        }
        
    }

    void OnDestroy()
    {
        if (!isDead) return; // 이미 Die() 처리되었으면 무시

        if (EnemySpawn.Instance != null)
            EnemySpawn.Instance.FiledEnemy = Mathf.Max(EnemySpawn.Instance.FiledEnemy - 1, 0);
    }

}
