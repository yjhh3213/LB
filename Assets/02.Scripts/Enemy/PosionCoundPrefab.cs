using UnityEngine;

public class PosionCoundPrefab : MonoBehaviour
{
    public float lifeTime = 6f;
    public float slowAmount = 1f;     // 이속 -1
    public float slowDuration = 5f;   // 5초 동안
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("독구름 트리거 감지: " + other.name);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 독구름 진입!");

            PlayerCtrl player = other.GetComponentInParent<PlayerCtrl>();
            if (player == null)
                player = other.GetComponentInChildren<PlayerCtrl>();
            if (player != null)
            {
                Debug.Log("슬로우 적용 시도");
                player.ApplySlow(slowAmount, slowDuration);
            }
            else
            {
                Debug.LogWarning("PlayerCtrl 컴포넌트를 못 찾음");
            }
        }
    }
}
