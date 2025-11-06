using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class ShotGun : MonoBehaviour
{
    //public Card card;                       // 카드 스크립트에 있는 변수를 얻어오기 위해 쓰임

    public GameObject QImage;               // 재장전 이미지
    public GameObject EImage;               // 재장전 이미지
    public GameObject RImage;               // 재장전 이미지

    public GameObject[] BulletPrefab;       // 탄
    public GameObject[] EmptyPrefab;        // 탄피
    public Transform FirePoint;             // 총구 위치
    public Transform EmptyBullet;           // 탄피 배출
    public GameObject[] BulletCount;        // 탄 갯수
    private int MaxBulletCount = 6;         // 최댓치 탄 갯수
    public int NowBulletCount = 6;          // 현재 탄 갯수
    public float BulletSpeed = 10.0f;       // 탄 속도
    public int count = 0;                   // 탄 이미지를 위한 카운트

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Conmand();

        // 카운트가 0 미만일 때 0으로 다시 정립하기
        if (count < 0)
        {
            count = 0;
        }
    }

    public float EmptyBulletSpeed = 0.0f;
    void Conmand()
    {
        //탄약 발사
        if (Input.GetMouseButtonDown(0) && Card.Instance.gameclick == false)
        {
            int ShotGunCardLevel = Card.Instance.ShotGunCard;
            int barrelCardLevel = Card.Instance.barrelCard;
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
                
                //print(decreasebullet);
                
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
                    }
                    EmptyBulletSpeed += Time.deltaTime;
                    Quaternion EBrot = EmptyBullet.rotation * Quaternion.Euler(0, 0, -EmptyBulletSpeed);
                    GameObject EB = Instantiate(EmptyPrefab[0], EmptyBullet.position, EBrot);
                    Destroy(EB, 2.0f);
                    if (BulletCount != null)
                    {
                        NowBulletCount--;
                        BulletCount[count].SetActive(false);
                    }
                    count++;
                }
            }
            else if (NowBulletCount <= 0)
            {
                print("Bullet Empty");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (NowBulletCount <= 0)
            {
                Reload();
                print("Reload");
            }
            else
            {
                print("아직 탄이 남았습니다");
            }
        }
    }

    public float ReloadTime = 3.0f;     // QTE가 진행되는 동안 재장전 시간
    bool isReloading = false;           // 재장전중인지
    /// </summary>
    public int qteMCount = 3;           // QTE 최대 횟수

    void Reload()
    {
        if (isReloading) return;

        // 총 탄약 수에서 현재 탄약 수를 뺀 나머지 탄약 수를 재장전한다
        int reloadBullet = MaxBulletCount - NowBulletCount;
        StartCoroutine(ReloadC(reloadBullet));
    }

    IEnumerator ReloadC(int reloadbullet)
    {
        isReloading = true;

        KeyCode[] qte = new KeyCode[] { KeyCode.Q, KeyCode.E, KeyCode.R };
        QImage.SetActive(true);
        EImage.SetActive(true);
        RImage.SetActive(true);

        for (int i = 0; i < qte.Length; i++)
        {
            float time = ReloadTime;
            bool IsSuccess = false;

            while (time > 0)
            {
                if (Input.anyKeyDown)
                {
                    bool correct = Input.GetKeyDown(qte[i]);

                    if (correct)
                    {
                        print("QTE 성공");
                        NowBulletCount += 2;
                        for (int j = 0; j < 2; j++)
                        {
                            BulletCount[count-1].SetActive(true);
                            count--;
                            //print(count);
                        }
                        IsSuccess = true;
                        if (qte[i] == KeyCode.Q) QImage.SetActive(false);
                        else if (qte[i] == KeyCode.E) EImage.SetActive(false);
                        else if (qte[i] == KeyCode.R) RImage.SetActive(false);
                        break;
                    }

                    else
                    {
                        foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
                        {
                            if (Input.GetKeyDown(code))
                            {
                                bool isKeyBoardKey = (
                                    code >= KeyCode.A && code <= KeyCode.Z);
                                if (isKeyBoardKey)
                                {
                                    if (code == KeyCode.Q || code == KeyCode.E)
                                    {
                                        print("QTE 실패!");
                                        isReloading = false;
                                        QImage.SetActive(false);
                                        EImage.SetActive(false);
                                        RImage.SetActive(false);
                                        yield break;
                                    }

                                    if (!qte.Contains(code))
                                    {
                                        print("QTE 실패!");
                                        isReloading = false;
                                        QImage.SetActive(false);
                                        EImage.SetActive(false);
                                        RImage.SetActive(false);
                                        yield break;
                                    }
                                }
                            }
                        }
                    }
                }

                time -= Time.deltaTime;
                yield return null;
            }

            if (!IsSuccess)
            {
                print("QTE 시간초과!");
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
        isReloading = false;

        QImage.SetActive(false);
        EImage.SetActive(false);
        RImage.SetActive(false);
    }
}


