using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite IdleSprite;                   // Idle.png
    public Sprite DashSprite;                   // Dash.png
    public Sprite DeathSprite;                  // Death.png
    public Sprite WalkSprite;                   // Walk.png
    
    public Sprite foot0;
    public Sprite foot1;
    public Sprite foot2;
    public Sprite foot3;
    public Sprite foot4;

    [Header("Stats")]
    public int health = 1;                      // 캐릭터 체력
    private bool dead = false;                  // 캐릭터 사망 여부
    bool isDashing = false;
    public float speed;                         // 캐릭터 속도
    public float Dash = 15.0f;                  // 캐릭터 대쉬 속도

    [Header("Transform")]
    public Transform body;
    public Transform foot;
    public Transform DashGauge;                 // 대쉬 게이지 바
    private SpriteRenderer bodyRenderer;        
    private SpriteRenderer footRenderer;        

    Vector2 moveV;                              // 캐릭터 조작키
    Vector2 dashdir;
    Rigidbody2D rb;                             // 캐릭터 물리
    bool DashUIOn = false;

    int nimblestepsCardLevel;                   // 기민한걸음
    int QuickstepCardLevel;                     // 퀵 스탭

    bool isSlowed = false;

    private Camera mainCamera; //이동 제한용 카메라 불러오기
    private float minX, maxX, minY, maxY;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SetBounds();//카메라 못나가기 세팅
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bodyRenderer.sprite = IdleSprite;

        footRenderer = transform.Find("foot").GetComponent<SpriteRenderer>();
        footRenderer.sprite = foot0;

        rb = GetComponent<Rigidbody2D>();
        dead = false;
    }

    public float dashCoolDown = 5.0f;           // 대쉬를 사용하기 위한 쿨타임
    public float dashTimer = 0f;                // 건들지 말 것
    public int dashcount = 1;                   // 대쉬 사용 횟수
    // Update is called once per frame
    void Update()
    {
        
        nimblestepsCardLevel = Card.Instance.nimblestepsCard;        // 기민한 걸음 Level
        QuickstepCardLevel = Card.Instance.QuickstepCard;            // 퀵 스탭     Level

        if (dead) return;                       // 죽었으면 입력 막기

        if (dashTimer > 0) 
            dashTimer -= Time.deltaTime;

        switch (nimblestepsCardLevel)
        {
            case 1:
                speed = 3.0f;
                break;
            case 2:
                speed = 4.0f;
                break;
            case 3:
                speed = 5.0f;
                break;
            default:
                speed = 2.0f;
                break;
        }

        switch (QuickstepCardLevel)
        {
            case 1:
                dashCoolDown = 4.0f;
                break;
            case 2:
                dashCoolDown = 3.0f;
                break;
            case 3:
                dashCoolDown = 2.0f;
                dashcount = 2;
                break;
            default:
                dashCoolDown = 5.0f;
                break;
        }

        if (!isDashing)
            ObjMove();

        UpdateSprite();
        DashGaugeUI();
        RestrictMovement();//카메라 밖으로 못나감
    }


    /// <summary>
    /// 대쉬 게이지 채우기
    /// </summary>
    float smoothY;

    void DashGaugeUI()
    {
        if (DashUIOn)
        {
            float ratio = Mathf.Clamp01(dashTimer / dashCoolDown);
            float current = DashGauge.localScale.y;
            float reversed = 1.0f - ratio;
            float target = reversed * 0.48f;
            if (target < 0.03f)
            {
                target = 0.0f;
            }

            float newY = Mathf.SmoothDamp(current, target, ref smoothY, 0.1f);

            //print(newY);

            DashGauge.localScale = new Vector3(0.4f, newY, 1.0f);

            // 위치 보정
            float originalHeight = 0.48f;
            float offset = (originalHeight - newY) / 2f;
            //print(offset);
            DashGauge.localPosition = new Vector3(DashGauge.localPosition.x, -offset, DashGauge.localPosition.z);
        }
        else {
            DashGauge.localScale = new Vector3(0.4f, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// 대쉬와 움직이기
    /// </summary>
    public float Walktime = 0.0f;

    void ObjMove()
    {
        if (!isDashing)
        {
            //W, A, S, D키 및 상하좌우키 이동 입력받기
            Vector2 Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveV = Move.normalized * speed;

            // foot 애니메이션 처리
            if (Move.magnitude > 0)
            {
                Walktime += Time.deltaTime;
                if(Walktime > 0.0f) footRenderer.sprite = foot1;
                if (Walktime > 0.15f) footRenderer.sprite = foot2;
                if (Walktime > 0.25f) { footRenderer.sprite = foot3; Walktime = 0.0f; }
            }
            else
            {
                footRenderer.sprite = foot0;
                Walktime = 0.0f;
            }
        }
        // 순간이동 Dash
        if (Input.GetMouseButtonDown(1) && dashTimer <= 0f /*&& dashcount != 0*/)
        {
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dir != Vector2.zero) // 방향 입력이 있을 때만 순간이동
            {
                DashUIOn = true;
                dashcount--;
                isDashing = true;
                dashdir = dir.normalized;
                dashTimer = dashCoolDown;
                bodyRenderer.sprite = DashSprite;
                print("Dash!");
                print(dashcount);
                StartCoroutine(DashTimerCoroutine());
                StartCoroutine(ReturnIdle(0.15f));  // 짧게 Dash Sprite 유지s
            }
        }
    }
    private void FixedUpdate()
    {
        if (dead) return;
        if (isDashing)
        {
            rb.MovePosition(rb.position + speed * dashdir * Dash * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + moveV * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 대쉬 타이머 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DashTimerCoroutine()
    {
        float dashDuration = 0.25f;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            elapsed += Time.fixedDeltaTime;

            // 잔상 생성 주기 조절
            StartCoroutine(CreateafterImage(0.25f, 3f));
            yield return new WaitForFixedUpdate();
        }
        /*if (QuickstepCardLevel >= 3) dashcount = 2;
        else dashcount = 1;*/
        isDashing = false;
    }

    /// <summary>
    /// 적과 충돌했을 때 게임 멈추기
    /// </summary>
    /// <param name="collision"></param>
    // collision Enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("aa") && !dead)
        {
            health--;
            Dead();
        }
    }

    void Dead()
    {
        dead = true;
        speed = 0.0f;
        Dash = 0.0f;
        bodyRenderer.sprite = DeathSprite;
        Time.timeScale = 0.0f;
    }

    void UpdateSprite()
    {
        if (dead) return;

        if(moveV.magnitude > 0.1f)
        {
            bodyRenderer.sprite = WalkSprite;
        }
        else
        {
            bodyRenderer.sprite = IdleSprite;
        }
    }

    IEnumerator ReturnIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!dead) bodyRenderer.sprite = IdleSprite;
    }

    void SetBounds() // 카메라 밖으로 못나가게 세팅
    {
        // 카메라 화면의 좌측 하단과 우측 상단을 월드 좌표로 변환
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // X, Y 이동 범위 설정
        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;
        minX -= 8f; maxX += 8f; minY -= 12f; maxY += 12f;//플레이어가 카메라 밖으로 못나가게 하기 위한 조치
    }
    void RestrictMovement() // 카메라 밖으로 못나가게
    {
        // 현재 플레이어 위치 가져오기
        Vector3 newPosition = transform.position;
        print(Mathf.Clamp(newPosition.x, minX, maxX));
        // 화면 경계를 넘어가지 않도록 Clamp 적용
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // 최종 위치 적용
        transform.position = newPosition;
    }
    /// <summary>
    /// 대쉬 잔상남기기
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="fadespeed"></param>
    /// <returns></returns>
    // Dash할 때 잔상남기기(진상 유지 시간, 사라지는 속도)
    IEnumerator CreateafterImage(float duration, float fadespeed)
    {
        GameObject afterImage = new GameObject("AfterImage");
        SpriteRenderer sr = afterImage.AddComponent<SpriteRenderer>();

        sr.sprite = bodyRenderer.sprite;
        sr.transform.position = body.position;
        sr.transform.localScale = body.localScale;
        sr.flipX = bodyRenderer.flipX;
        sr.sortingOrder = bodyRenderer.sortingOrder - 1;        // 본체보다 뒤에 배치

        Color color = bodyRenderer.color;
        sr.color = new Color(color.r, color.g, color.b, 0.8f);
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            sr.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0.8f, 0f, t));
            elapsed += Time.deltaTime * fadespeed;
            yield return null;
        }

        Destroy(afterImage);
    }

    /*IEnumerator SpwanImage()
    {
        for(int i = 0; i < 10; i++)
        {
            StartCoroutine(CreateafterImage(0.5f, 5f));
            yield return new WaitForSeconds(0.02f);
        }
    }*/

    // 2025 - 11 - 23 베놈좀비 함수 추가 
    Coroutine slowRoutine;
    public void ApplySlow(float amount, float duration)
    {
        if (slowRoutine != null) return;
        slowRoutine = StartCoroutine(SlowCoroutine(amount, duration));
    }

    IEnumerator SlowCoroutine(float amount, float duration)
    {
        if (isSlowed) yield break;

        speed -= amount;
        Debug.Log("느려짐! 현재 이속: " + speed);

        yield return new WaitForSeconds(duration);

        speed += amount;
        Debug.Log("슬로우 해제! 현재 이속: " + speed);

        isSlowed = false;
    }
}
