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

    [Header("애니메이션 컨트롤러 그룹")]
    public AnimatorGroup[] animatorGroups;

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
        if (animationEffectPrefab == null)
        {
            Debug.LogError("animationEffectPrefab이 할당되지 않았습니다!");
            return null;
        }

        // EffectManager에서 해당 이름의 애니메이션 컨트롤러 찾기
        RuntimeAnimatorController selectedController = null;
        foreach (var group in animatorGroups)
        {
            if (group.groupName == animationName)
            {
                selectedController = group.animatorController;
                break;
            }
        }

        if (selectedController == null)
        {
            Debug.LogWarning($"애니메이션 컨트롤러 '{animationName}'을 찾을 수 없습니다!");
            return null;
        }

        GameObject effect = Instantiate(animationEffectPrefab, position, Quaternion.identity);

        // 컴포넌트 가져오기
        Animator animator = effect.GetComponent<Animator>();
        SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();

        if (animator == null || spriteRenderer == null)
        {
            Debug.LogError("Animator 또는 SpriteRenderer가 프리팹에 없습니다!");
            Destroy(effect);
            return null;
        }

        // 애니메이션 설정
        animator.runtimeAnimatorController = selectedController;
        animator.speed = animSpeed;

        // 페이드 아웃 시작
        StartCoroutine(AnimationEffectRoutine(effect, spriteRenderer, destroyTime, fadeStartTime));

        return effect;
    }

    // 랜덤 이펙트 생성
    public GameObject PlayRandom(string spriteName, Vector3 position,
        float destroyTime = 2f, float fadeStartTime = 0.5f)
    {
        if (randomEffectPrefab == null)
        {
            Debug.LogError("randomEffectPrefab이 할당되지 않았습니다!");
            return null;
        }

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

        // 컴포넌트 가져오기
        SpriteRenderer spriteRenderer = effect.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer가 프리팹에 없습니다!");
            Destroy(effect);
            return null;
        }

        // 스프라이트 설정
        spriteRenderer.sprite = selectedSprite;

        // 페이드 아웃 시작
        StartCoroutine(RandomEffectRoutine(effect, spriteRenderer, destroyTime, fadeStartTime));

        return effect;
    }

    // 애니메이션 이펙트 처리 코루틴
    IEnumerator AnimationEffectRoutine(GameObject effect, SpriteRenderer spriteRenderer,
    float destroyTime, float fadeStartTime)
    {
        float fadeDuration = destroyTime - fadeStartTime;

        // 페이드 시작 전까지 대기
        float elapsed = 0f;

        while (elapsed < fadeStartTime)
        {
            if (effect == null || spriteRenderer == null)
                yield break;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 페이드 아웃
        elapsed = 0f;
        Color color = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            //코루틴 중 SpriteRenderer가 사라지면 즉시 종료
            if (effect == null || spriteRenderer == null)
                yield break;

            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        if (effect != null)
            Destroy(effect);
    }


    // 랜덤 이펙트 처리 코루틴
    IEnumerator RandomEffectRoutine(GameObject effect, SpriteRenderer spriteRenderer,
    float destroyTime, float fadeStartTime)
    {
        float fadeDuration = destroyTime - fadeStartTime;

        // 페이드 시작 전 대기
        float elapsed = 0f;
        while (elapsed < fadeStartTime)
        {
            if (effect == null || spriteRenderer == null)
                yield break;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 페이드 아웃
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            //이 부분이 핵심!
            if (effect == null || spriteRenderer == null)
                yield break;

            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Color c = spriteRenderer.color;

            spriteRenderer.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        if (effect != null)
            Destroy(effect);
    }

}

// 애니메이션 컨트롤러 그룹 (Inspector에서 설정)
[System.Serializable]
public class AnimatorGroup
{
    public string groupName;
    public RuntimeAnimatorController animatorController;
}

// 스프라이트 그룹 (Inspector에서 설정)
[System.Serializable]
public class SpriteGroup
{
    public string groupName;
    public Sprite[] sprites;
}