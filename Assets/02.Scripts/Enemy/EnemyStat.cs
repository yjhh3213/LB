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
    public bool isDead = false;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigidbody;
    public float dieAnimTime = 0.7f;
    public GameObject poisonCloundPrefab;

    private SpriteRenderer HandspriteRenderer;
    private SpriteRenderer FeetspriteRenderer;
    public GameObject Hand; //손
    public GameObject Feet; //발
    private void Start()
    {
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
            HandspriteRenderer = Hand.GetComponentInChildren<SpriteRenderer>();
            FeetspriteRenderer = Feet.GetComponentInChildren<SpriteRenderer>();
        }
        if (rigidbody == null)
        {
            rigidbody = GetComponentInChildren<Rigidbody2D>();
        }
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator를 찾지 못했습니다! Enemy 프리팹에 Animator 컴포넌트가 있는지 확인하세요.");
        }
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;

        transform.position += dir * EnemySpeed * Time.deltaTime;

        // 몬스터가 플레이어 방향 바라보게 하는 코드 
        float diffx = player.position.x - transform.position.x;

        Vector3 handPosition = transform.position;

        if (diffx > 0f) // 플레이어가 오른쪽에 있을 때 (몬스터가 오른쪽을 바라볼 때)
        {
            spriteRenderer.flipX = false;

            handPosition.x += 0.25f;
            handPosition.y -= 0.2f;
            Hand.transform.position = handPosition;

            HandspriteRenderer.flipY = false; // 90도 돌아가있음
            FeetspriteRenderer.flipX = false;
        }
        else if (diffx < 0f) // 플레이어가 왼쪽에 있을 때 (몬스터가 왼쪽을 바라볼 때)
        {
            spriteRenderer.flipX = true;

            handPosition.x -= 0.25f;
            handPosition.y -= 0.2f;
            Hand.transform.position = handPosition;

            HandspriteRenderer.flipY = true; // 90도 돌아가있음
            FeetspriteRenderer.flipX = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
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
        if (isDead) return; // 두 번 실행 방지
        isDead = true;

        if (EnemySpawn.Instance != null)
        {
            EnemySpawn.Instance.FiledEnemy = Mathf.Max(EnemySpawn.Instance.FiledEnemy - 1, 0);
        }
        SoundManager.Instance.Player_SFX(5);

        EnemyBack back = GetComponent<EnemyBack>(); // 뒤로 밀리다가 죽으면 멈추게 
        if (back != null)
            back.StopKnockback();
        if (rigidbody != null)
        {
            rigidbody.simulated = false; //리지드바디 비활성화
        }
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
        Destroy(Hand);
        Destroy(Feet);
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
