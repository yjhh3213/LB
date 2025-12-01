using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton_tlcp : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private float forceMin = 3f;
    [SerializeField] private float forceMax = 7f;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite[] debrisSprites; // Inspector에서 할당
    [SerializeField] private int spriteIndex = 0; // 생성 시 설정할 인덱스

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 컴포넌트 초기화
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Rigidbody2D가 없으면 추가
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // SpriteRenderer가 없으면 추가
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

        // 2초 후 제거
        Destroy(gameObject, lifetime);
    }

    void ApplyRandomForce()
    {
        // 360도 랜덤 각도
        float randomAngle = Random.Range(0f, 360f);

        // 랜덤 힘의 세기
        float forceMagnitude = Random.Range(forceMin, forceMax);

        // 각도를 방향 벡터로 변환
        Vector2 forceDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        );

        // 힘 적용
        Vector2 force = forceDirection * forceMagnitude;
        rb.AddForce(force, ForceMode2D.Impulse);

        // 랜덤 회전력 추가 (선택사항)
        float randomTorque = Random.Range(-5f, 5f);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);
    }

    // 스프라이트 인덱스 설정 메서드
    public void SetSpriteIndex(int index)
    {
        spriteIndex = index;
    }

    // 스프라이트 배열 설정 메서드
    public void SetDebrisSprites(Sprite[] sprites)
    {
        debrisSprites = sprites;
    }
}