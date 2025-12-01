using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using TMPro;
using UnityEditor.UIElements;

public class ShotGun : MonoBehaviour
{
    //public Card card;                     // 카드 스크립트에 있는 변수를 얻어오기 위해 쓰임
    int fastdrawLevel;                      // 패스트 드로우 Level
    int weaknessCardLevel;                  // 약점 포착 Level

    [Header("GameObject")]
    public PlayerCtrl playerCtrl;           // 플레이어
    public GameObject ReloadImage;          // 재장전 이미지
    public TMP_Text qteText;
    public GameObject[] BulletPrefab;       // 탄
    public GameObject[] EmptyPrefab;        // 탄피
    public GameObject[] BulletCount;        // 탄 갯수

    [Header("Transform")]
    public Transform FirePoint;             // 총구 위치
    public Transform EmptyBullet;           // 탄피 배출

    [Header("States")]
    private int MaxBulletCount = 6;         // 최댓치 탄 갯수
    public int NowBulletCount = 6;          // 현재 탄 갯수
    public float BulletSpeed = 10.0f;       // 탄 속도
    public int count = 0;                   // 탄 이미지를 위한 카운트
    public float ReloadTime = 3.0f;         // QTE가 진행되는 동안 재장전 시간
    public float WaitShoot = 1.0f;          // 다음 공격까지 걸리는 시간
    public float WaitEmptyBullet = 0.5f;    // 빈 탄 나오기까지 걸리는 시간
    bool WaitEmptySccess = false;
    bool isReloading = false;               // 재장전중인지

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCtrl.dead)
        {
            fastdrawLevel = Card.Instance.fastdraw;

            Conmand();

            // 카운트가 0 미만일 때 0으로 다시 정립하기
            if (count < 0)
            {
                count = 0;
            }
        }
    }

    public float EmptyBulletSpeed = 0.0f;
    void Conmand()
    {
        if (weaknessCardLevel >= 3)
        {
            float Delay = WaitShoot * 0.2f;
            float WaitDelay = WaitShoot - Delay;
            WaitDelay -= Time.deltaTime;
            print("WaitDelay : " + WaitDelay);
        }
        else { WaitShoot -= Time.deltaTime; }

        if (WaitEmptySccess)
        {
            WaitEmptyBullet -= Time.deltaTime;

            if (WaitEmptyBullet <= 0.0f)
            {
                SoundManager.Instance.Player_SFX(1);
                EmptyBulletSpeed += Time.deltaTime;
                Quaternion EBrot = EmptyBullet.rotation * Quaternion.Euler(0, 0, -EmptyBulletSpeed);
                GameObject EB = Instantiate(EmptyPrefab[0], EmptyBullet.position, EBrot);
                // 탄피에 힘 추가
                Rigidbody2D rb = EB.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 force = new Vector2(Random.Range(-2f, 1f), Random.Range(5f, 7f));// 위쪽과 약간의 옆방향 힘
                    rb.AddForce(force, ForceMode2D.Impulse);
                    rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);// 회전력 추가 (더 현실적으로)
                }
                SoundManager.Instance.Player_SFX(7);
                Destroy(EB, 2.0f);
                WaitEmptySccess = false;
                WaitEmptyBullet = 0.5f;
            }
        }
        //탄약 발사
        if (Input.GetMouseButtonDown(0) && Card.Instance.gameclick == false && WaitShoot <=0.0f)
        {
            //print(WaitShoot);
            int ShotGunCardLevel = Card.Instance.ShotGunCard;           // 샷건개조 Level
            int barrelCardLevel = Card.Instance.barrelCard;             // 총열개조 Level
            //print("Left Click");
            if (NowBulletCount >= 1)
            {
                int BulletNumber = Random.Range(3, 6);
                //print("더하기 전 : " + BulletNumber);
                switch (ShotGunCardLevel)
                {
                    case 1:
                        BulletNumber += 1;
                        break;
                    case 2:
                        BulletNumber += 2;
                        break;
                    case 3:
                        BulletNumber += 4;
                        break;
                    default:
                        break;
                }
                //print("후 : " + BulletNumber);


                float decreasebullet = 0.0f;

                switch (barrelCardLevel)
                {
                    case 1:
                        decreasebullet = 15.0f - (0.2f * 15.0f);
                        break;
                    case 2:
                        decreasebullet = 15.0f - (0.4f * 15.0f);
                        break;
                    case 3:
                        decreasebullet = 15.0f - (0.6f * 15.0f);
                        break;
                    default:
                        decreasebullet = 15.0f;
                        break;
                }

                EffectManager.Instance.PlayAnimation("총염", FirePoint.position, 1f, 0.25f, 0.1f); // 이펙트 생성
                EffectManager.Instance.PlayAnimation("총연기", transform.position + new Vector3(0,+0.5f,0), 1f, 0.5f, 0.25f); // 이펙트 생성
                if (BulletPrefab != null)
                {
                    for (int i = 0; i < BulletNumber; i++)
                    {
                        float BulletSpread = Random.Range(-decreasebullet, decreasebullet);
                        Quaternion bulletRot = FirePoint.rotation * Quaternion.Euler(0, 0, BulletSpread);

                        GameObject Bullet = Instantiate(BulletPrefab[0], FirePoint.position, bulletRot);

                        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
                        rb.velocity = bulletRot * Vector3.right * BulletSpeed;
                        Destroy(Bullet, 2.0f);

                        WaitEmptySccess = true;
                    }
              
                    if (BulletCount != null)
                    {
                        NowBulletCount--;
                        BulletCount[count].SetActive(false);
                        WaitShoot = 1.0f;
                    }
                    SoundManager.Instance.Player_SFX(0);
                    count++;
                }
            }
            else if (NowBulletCount <= 0) //총알 없음
            {
                SoundManager.Instance.Player_SFX(6);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            //print("Reload");
        }
    }

    void Reload()
    {
        if (isReloading) return;
        FindObjectOfType<Aim>().aim_ch("장전"); //에임 모양 변경
        // 총 탄약 수에서 현재 탄약 수를 뺀 나머지 탄약 수를 재장전한다
        int reloadBullet = MaxBulletCount - NowBulletCount;
        StartCoroutine(ReloadC(reloadBullet));
    }

    IEnumerator ReloadC(int reloadbullet)
    {
        isReloading = true;

        // LEVEL 3 이상일 때 즉시 재장전
        if (fastdrawLevel >= 3)
        {
            NowBulletCount += 6;
            for (int j = 0; j < 6; j++)
            {
                if (count > 0)
                {
                    BulletCount[count - 1].SetActive(true);
                    count--;
                }
                //print(count);
            }
            isReloading = false;
            FindObjectOfType<Aim>().aim_ch("일반"); //에임 모양 변경
            yield break;
        }

        // LEVEL 0~2 QTE 처리

        yield return null;

        if (qteText != null)
        {
            qteText.gameObject.SetActive(true);
        }
        else
        {
            yield break;
        }

        ReloadImage.SetActive(true);

        int qteSteps = 3;

        //전체에서 QERT만 사용하기
        KeyCode[] QET = { KeyCode.Q, KeyCode.E, KeyCode.T };

        for (int i = 0; i < qteSteps; i++)
        {
            KeyCode targetKey = QET[Random.Range(0, QET.Length)];

            qteText.text = targetKey.ToString();
            print($"QTE {i + 1}/{qteSteps} : {targetKey} 키를 누르세요");

            float time = ReloadTime; // 각 단계별 시간
            bool IsSuccess = false;
            bool isFailed = false;

            // !Card.Instance.keyboard를 추가하므로써 Q, E, T를 눌러도 장전안되게 만듦
            while (time > 0 && !isFailed && !Card.Instance.keyboard)
            {
                if (Input.GetKeyDown(targetKey))
                {
                    IsSuccess = true;

                    int bulletreload = 0;

                    switch (fastdrawLevel)
                    {
                        case 1:
                            bulletreload = 3;
                            break;
                        case 2:
                            bulletreload = 6;
                            break;
                        default:
                            bulletreload = 2;
                            break;
                    }
                    int ReloadAmount = Mathf.Min(bulletreload, MaxBulletCount - NowBulletCount);

                    if (ReloadAmount <= 0)
                    {
                        print("Full Bullet");
                    }
                    else
                    {
                        NowBulletCount += ReloadAmount;

                        for (int j = 0; j < ReloadAmount; j++)
                        {
                            if (count > 0)
                            {
                                SoundManager.Instance.Player_SFX(2);
                                BulletCount[count - 1].SetActive(true);
                                count--;
                            }
                        }
                    }
                    break;
                }
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode key in QET)
                    {
                        if (key != targetKey && Input.GetKeyDown(key))
                        {
                            print("QTE 실패! (틀린 키 입력)");
                            isFailed = true; // 실패 처리
                            break; // for 루프 탈출
                        }
                    }
                }

                if (isFailed) break;

                time -= Time.deltaTime;
                yield return null;
            }

            // 단일 QTE 결과 판정
            if (isFailed || !IsSuccess)
            {
                if (!isFailed)
                {
                    print("QTE TimeOver");
                }
                print("Stop Reload");
                break;
            }

            if (NowBulletCount >= MaxBulletCount)
            {
                print("Full Bullet");
                break;
            }

            // 성공 : 다음 단계를 위해 0.1초 대기
            yield return new WaitForSeconds(0.1f);
        }

        ReloadImage.SetActive(false);
        qteText.gameObject.SetActive(false);
        isReloading = false;
        FindObjectOfType<Aim>().aim_ch("일반"); //에임 모양 변경
    }
}