using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton_tlcp : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private float forceMin = 3f;
    [SerializeField] private float forceMax = 7f;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite[] debrisSprites;
    [SerializeField] private int spriteIndex = 0;

    [Header("Motion Settings")]
    [SerializeField] private float explosionTime = 0.5f; // 날아가는 시간
    [SerializeField] private float returnTime = 1.5f; // 모이는 시간

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public GameObject qhscp;//본체 위치
    private Vector3 spawnPosition; // 생성 위치 기억
    private float initialRotation; // 초기 회전값
    private float currentRotation; // 현재 회전값
    private bool isReturning = false;
    private Coroutine returnCoroutine; // 코루틴 추적

    void Start()
    {
        initialRotation = transform.rotation.eulerAngles.z;

        // 컴포넌트 초기화
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // 중력 활성화
        rb.gravityScale = 1f;

        // 스프라이트 설정
        if (debrisSprites != null && spriteIndex < debrisSprites.Length)
        {
            spriteRenderer.sprite = debrisSprites[spriteIndex];
        }

        // 랜덤 힘 적용
        ApplyRandomForce();

        // 0.5초 후 정지하고 돌아가기 시작
        returnCoroutine = StartCoroutine(ExplosionSequence());
    }
    private void Update()
    {
        // 생각해보니 넉백될꺼니깐 실시간으로 받으면 될듯
        if (qhscp!) spawnPosition = qhscp.transform.position;
    }
    void ApplyRandomForce()
    {
        float randomAngle = Random.Range(0f, 360f);
        float forceMagnitude = Random.Range(forceMin, forceMax);

        Vector2 forceDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        );

        Vector2 force = forceDirection * forceMagnitude;
        rb.AddForce(force, ForceMode2D.Impulse);

        float randomTorque = Random.Range(-5f, 5f);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);
    }

    IEnumerator ExplosionSequence()
    {
        // 0.5초 동안 날아감
        yield return new WaitForSeconds(explosionTime);

        // 현재 회전값 저장
        currentRotation = transform.rotation.eulerAngles.z;

        // 정지
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f; // 중력 끄기
        rb.isKinematic = true; // Kinematic으로 전환

        isReturning = true;

        // 1.5초 동안 원래 위치로 이동
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < returnTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / returnTime;

            // 부드러운 이동을 위한 easeInOut 곡선
            t = t * t * (3f - 2f * t);

            // 위치 이동
            transform.position = Vector3.Lerp(startPosition, spawnPosition, t);

            // 역회전 (현재 회전에서 초기 회전으로)
            float rotation = Mathf.LerpAngle(currentRotation, initialRotation, t);
            transform.rotation = Quaternion.Euler(0, 0, rotation);

            yield return null;
        }

        // 정확히 원래 위치 및 회전으로
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(0, 0, initialRotation);

        // 오브젝트 제거
        Destroy(gameObject);
    }

    public void SetSpriteIndex(int index)
    {
        spriteIndex = index;
    }

    public void SetDebrisSprites(Sprite[] sprites)
    {
        debrisSprites = sprites;
    }

    // 해골이 죽었을 때 호출: 정지하고 중력으로 떨어뜨림
    public void StopAndFall()
    {
        // 진행 중인 코루틴 중단
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        // Rigidbody를 다시 활성화
        rb.isKinematic = false;
        rb.gravityScale = 1f;

        // 현재 속도 제거 (이동 정지)
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // 2초 후 제거
        Destroy(gameObject, 2f);
    }
}