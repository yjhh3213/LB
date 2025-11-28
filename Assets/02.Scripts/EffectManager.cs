using System.Collections;
using UnityEngine;

// 사용 예시:
// EffectManager.Instance.PlayAnimation("Explosion", transform.position, 1.5f, 3f, 1f);
// EffectManager.Instance.PlayRandom("Hit", transform.position, 2f, 0.5f);

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [Header("프리팹")]
    public GameObject animationEffectPrefab;
    public GameObject randomEffectPrefab;

    [Header("랜덤 스프라이트 그룹")]
    public SpriteGroup[] spriteGroups;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 애니메이션 이펙트 생성
    public GameObject PlayAnimation(string animationName, Vector3 position, float animSpeed = 1f,
        float destroyTime = 3f, float fadeStartTime = 1f)
    {
        GameObject effect = Instantiate(animationEffectPrefab, position, Quaternion.identity);
        AnimationEffect anim = effect.GetComponent<AnimationEffect>();
        anim.Setup(animationName, animSpeed, destroyTime, fadeStartTime);
        return effect;
    }

    // 랜덤 이펙트 생성
    public GameObject PlayRandom(string spriteName, Vector3 position,
        float destroyTime = 2f, float fadeStartTime = 0.5f)
    {
        // EffectManager에서 해당 이름의 스프라이트 그룹 찾기
        Sprite selectedSprite = null;
        foreach (var group in spriteGroups)
        {
            if (group.groupName == spriteName)
            {
                selectedSprite = group.sprites[Random.Range(0, group.sprites.Length)];
                break;
            }
        }

        if (selectedSprite == null)
        {
            Debug.LogWarning($"스프라이트 그룹 '{spriteName}'을 찾을 수 없습니다!");
            return null;
        }

        GameObject effect = Instantiate(randomEffectPrefab, position, Quaternion.identity);
        RandomEffect random = effect.GetComponent<RandomEffect>();
        random.Setup(selectedSprite, destroyTime, fadeStartTime);
        return effect;
    }
}

// 애니메이션 이펙트
public class AnimationEffect : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float destroyTime;
    private float fadeStartTime;
    private float fadeDuration;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(string animationName, float animSpeed, float destroyTime, float fadeStartTime)
    {
        this.destroyTime = destroyTime;
        this.fadeStartTime = fadeStartTime;
        this.fadeDuration = destroyTime - fadeStartTime;

        // 애니메이션 설정
        animator.speed = animSpeed;
        animator.Play(animationName);

        StartCoroutine(EffectRoutine());
    }

    IEnumerator EffectRoutine()
    {
        // 페이드 시작 전까지 대기
        yield return new WaitForSeconds(fadeStartTime);

        // 페이드 아웃
        float elapsed = 0f;
        Color color = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // 제거
        Destroy(gameObject);
    }
}

// 랜덤 이펙트
public class RandomEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float destroyTime;
    private float fadeStartTime;
    private float fadeDuration;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(Sprite sprite, float destroyTime, float fadeStartTime)
    {
        this.destroyTime = destroyTime;
        this.fadeStartTime = fadeStartTime;
        this.fadeDuration = destroyTime - fadeStartTime;

        // 스프라이트 설정
        spriteRenderer.sprite = sprite;

        StartCoroutine(EffectRoutine());
    }

    IEnumerator EffectRoutine()
    {
        // 페이드 시작 전까지 대기
        yield return new WaitForSeconds(fadeStartTime);

        // 페이드 아웃
        float elapsed = 0f;
        Color color = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // 제거
        Destroy(gameObject);
    }
}

// 스프라이트 그룹 (Inspector에서 설정하기 쉽도록)
[System.Serializable]
public class SpriteGroup
{
    public string groupName;
    public Sprite[] sprites;
}