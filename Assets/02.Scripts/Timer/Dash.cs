using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public float dashCooldown = 3f;       // 쿨타임 (초)
    private float currentCooldown = 0f;   // 남은 쿨타임

    public Image cooldownImage;           // 쿨타임 UI (Radial Fill)

    void Start()
    {
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f; // 시작할 땐 비워둠 (사용 가능 상태)
    }

    void Update()
    {
        // 쿨타임 감소
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;

            if (cooldownImage != null)
                cooldownImage.fillAmount = currentCooldown / dashCooldown;
        }
        else
        {
            if (cooldownImage != null)
                cooldownImage.fillAmount = 0f; // 쿨타임 끝 → 게이지 비움

            // R 키 입력 감지
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCooldown();
            }
        }
    }

    void StartCooldown()
    {
        Debug.Log("쿨타임 시작!");
        currentCooldown = dashCooldown;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f; // 꽉 채우기 → 줄어들기 시작
    }
}
