using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShotGun : MonoBehaviour
{
    public GameObject QImage;               // 재장전 이미지
    public GameObject EImage;               // 재장전 이미지
    public GameObject RImage;               // 재장전 이미지

    public GameObject[] BulletPrefab;       // 탄
    public GameObject[] EmptyPrefab;        // 탄피
    public Transform FirePoint;             // 총구 위치
    public Transform EmptyBullet;           // 탄피 배출
    public Text BulletCount;                // 탄 갯수
    private int MaxBulletCount = 6;         // 최댓치 탄 갯수
    public int NowBulletCount = 0;          // 현재 탄 갯수
    public float BulletSpeed = 10.0f;       // 탄 속도

    // Start is called before the first frame update
    void Start()
    {
        BulletCount.text = NowBulletCount.ToString()+ " "+ "/" + " " + MaxBulletCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Conmand();
        BulletCount.text = NowBulletCount.ToString() + " " + "/" + " " + MaxBulletCount.ToString();
    }

    public float EmptyBulletSpeed = 0.0f;
    void Conmand()
    {
        //탄약 발사
        if (Input.GetMouseButtonDown(0))
        {
            print("Left Click");
            if(NowBulletCount >= 1) 
            {
                int BulletNumber = Random.Range(3, 9);
                //print(BulletNumber);
                if (BulletPrefab != null)
                {
                    for (int i = 0; i < BulletNumber; i++)
                    {
                        float BulletSpread = Random.Range(-15f, 15f);
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
                    NowBulletCount--;
                }
            }
            else if(NowBulletCount <= 0)
            {
                print("Bullet Empty");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(NowBulletCount <= 0)
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
            
            while(time > 0)
            {
                if (Input.anyKeyDown)
                {
                    bool correct = Input.GetKeyDown(qte[i]);

                    if (correct)
                    {
                        print("QTE 성공");
                        NowBulletCount += 2;
                        IsSuccess = true;
                        if (qte[i] == KeyCode.Q) QImage.SetActive(false);
                        else if (qte[i] == KeyCode.E) EImage.SetActive(false);
                        else if (qte[i] == KeyCode.R) RImage.SetActive(false);
                        break;
                    }
                    
                    else
                    {
                        foreach(KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
                        {
                            if (Input.GetKeyDown(code))
                            {
                                bool isKeyBoardKey = (
                                    code >= KeyCode.A && code <= KeyCode.Z);
                                if (isKeyBoardKey)
                                {
                                    if(code == KeyCode.Q || code == KeyCode.E || code == KeyCode.T)
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


