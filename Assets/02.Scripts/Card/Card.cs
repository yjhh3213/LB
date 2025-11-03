using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CountTimer ct;

    public static Card Instance;

    public bool gameclick = false;      // 게임 클릭 되는 걸 방지
    public GameObject[] SetCardobj;     // 카드오브젝트
    public Sprite[] CardImage;          // 카드 이미지
    public int ShotGunCard = 0;         // 샷건개조
    public int BulletCard = 0;          // 총알개조
    public int barrelCard = 0;          // 총열개조
    public int weaknessCard = 0;        // 약점포착
    public int nimblestepsCard = 0;     // 기민한걸음
    public int QuickstepCard = 0;       // 퀵 스탭
    public int fastdraw = 0;            // 빠른 장전

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cardbuff();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    int Sect1 = 0;      // SetCardobj0번째
    int Sect2 = 0;      // SetCardobj1번째
    int Sect3 = 0;      // SetCardobj2번째
    void cardbuff()
    {
        Time.timeScale = 0.0f;
        gameclick = true;
        if (CardImage != null && SetCardobj != null)
        {
            for(int i = 0; i < 3; i++)
            {
                int num = Random.Range(2, 3);
                Image cardDisplay = SetCardobj[i].GetComponent<Image>();

                if(cardDisplay != null)
                {
                    cardDisplay.sprite = CardImage[num];
                }

                if(i == 0)
                {
                    Sect1 = num;
                    //print(Sect1);
                }
                else if (i == 1)
                {
                    Sect2 = num;
                    //print(Sect2);
                }
                else if (i == 2)
                {
                    Sect3 = num;
                    //print(Sect3);
                }
                SetCardobj[i].SetActive(true);
            }
        }
    }

    // 카드를 눌렀을 때 해당하는 카드의 능력치 올리기
    public void Test()
    {
        if(Sect1 == 0 || Sect2 == 0 || Sect3 == 0)
        {
            ShotGunCard++;
            //print("1번째카드 활성화!");
        }
        else if(Sect1 == 1 || Sect2 == 1 || Sect3 == 1)
        {
            BulletCard++;
            //print("2번째카드 활성화!");
        }
        else if (Sect1 == 2 || Sect2 == 2 || Sect3 == 2)
        {
            barrelCard++;
            print("3번째카드 활성화!");
        }
        else if (Sect1 == 3 || Sect2 == 3 || Sect3 == 3)
        {
            weaknessCard++;
            print("4번째카드 활성화!");
        }
        else if (Sect1 == 4 || Sect2 == 4 || Sect3 == 4)
        {
            nimblestepsCard++;
            print("5번째카드 활성화!");
        }
        else if (Sect1 == 5 || Sect2 == 5 || Sect3 == 5)
        {
            QuickstepCard++;
            print("6번째카드 활성화!");
        }
        else if (Sect1 == 6 || Sect2 == 6 || Sect3 == 6)
        {
            fastdraw++;
            print("7번째카드 활성화!");
        }
        NoSeeCard();
    }

    // 카드 보이지 않게 하기
    void NoSeeCard()
    {
        for (int i = 0; i < 3; i++)
        {
            SetCardobj[i].SetActive(false);
        }
        gameclick = false;
        Time.timeScale = 1.0f;
    }
}
