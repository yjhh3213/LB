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
    public float dieAnimTime = 0.01f;
    public GameObject poisonCloundPrefab;

    private SpriteRenderer HandspriteRenderer;
    private SpriteRenderer FeetspriteRenderer;
    public GameObject Hand; //손
    public GameObject Feet; //발

    [Header("사망 효과")]
    private float deathDelay = 6f;        // 사망 후 페이드 시작까지 대기 시간
    private float fadeDuration = 3f;      // 페이드 아웃 지속 시간
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
        for (int i = 0; i < Random.Range(1, 3); i++) EffectManager.Instance.PlayRandom("피_2", transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f), 3f, 1.5f); // 이펙트 생성
        for (int i = 0; i < Random.Range(3, 5); i++) EffectManager.Instance.PlayRandom("피_3", transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f), 8f, 6.5f); // 이펙트 생성
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
        EnemySpeed = 0;
        EnemyBack back = GetComponent<EnemyBack>(); // 뒤로 밀리다가 죽으면 멈추게 
        SoundManager.Instance.Player_SFX(5);
        GameManager gm = FindObjectOfType<GameManager>(); //킬카운트 증가

        
        if (rigidbody != null)              rigidbody.simulated = false; //리지드바디 비활성화
        if (poisonCloundPrefab != null)     Instantiate(poisonCloundPrefab, transform.position, Quaternion.identity);
        if (anim != null)                   anim.SetBool("Die" , true);
        if (EnemySpawn.Instance != null)    EnemySpawn.Instance.OnEnemyDied();
        if (back != null)                   back.StopKnockback();
        if (gm != null) gm.killCount++;

        Destroy(Hand);
        Destroy(Feet);

        StartCoroutine(HandleDeath());
    }
    IEnumerator HandleDeath()
    {
        // 6초 대기
        yield return new WaitForSeconds(deathDelay);

        // 3초에 걸쳐 페이드 아웃
        float elapsed = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // 투명도가 0이 되면 오브젝트 제거
        Destroy(gameObject);
    }


    void OnDestroy()
    {
        if (!isDead) return; // 이미 Die() 처리되었으면 무시
        
        
    }

}
