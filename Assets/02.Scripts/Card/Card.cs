using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CountTimer ct;

    public static Card Instance;

    [Header("GameObjet")]
    public GameObject[] SetCardobj;     // 카드오브젝트
    public GameObject Player;           // 플레이어
    public Sprite[] CardImage;          // 카드 이미지

    [Header("CardStates")]
    public bool gameclick = false;      // 게임 클릭 되는 걸 방지
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

    List<int> Arraynum;
    void cardbuff()
    {
        Time.timeScale = 0.0f;
        Player.SetActive(false);
        gameclick = true;

        Arraynum = new List<int> { 0, 1, 2, 3};

        if (ShotGunCard >= 3) Arraynum.Remove(0);
        if (BulletCard >= 2) Arraynum.Remove(1);
        if (barrelCard >= 3) Arraynum.Remove(2);
        if (weaknessCard >= 3) Arraynum.Remove(3);
        /*if (nimblestepsCard >= 1) Arraynum.Remove(4);
        if (QuickstepCard >= 1) Arraynum.Remove(5);
        if (fastdraw >= 1) Arraynum.Remove(6);*/

        if (CardImage != null && SetCardobj != null)
        {
            for(int i = 0; i < 3; i++)
            {
                if (Arraynum.Count == 0)
                {
                    Debug.Log("더 이상 선택할 카드 없음");
                    NoSeeCard();
                    return;
                }

                int randIndex = Random.Range(0, Arraynum.Count);
                int num = Arraynum[randIndex];

                Image cardDisplay = SetCardobj[i].GetComponent<Image>();
                if (cardDisplay != null) cardDisplay.sprite = CardImage[num];

                if(i == 0)
                {
                    Sect1 = num;
                    print("Sect1 : "+ Sect1);
                }
                else if (i == 1)
                {
                    Sect2 = num;
                    print("Sect2 : " + Sect2);
                }
                else if (i == 2)
                {
                    Sect3 = num;
                    print("Sect3 : " + Sect3);
                }
                SetCardobj[i].SetActive(true);
            }
        }
    }

    // 카드를 눌렀을 때 해당하는 카드의 능력치 올리기
    public void SelectCard(int index)
    {
        int cardType = 0;

        if (index == 0) cardType = Sect1;
        else if (index == 1) cardType = Sect2;
        else if (index == 2) cardType = Sect3;

        switch (cardType)
        {
            case 0: ShotGunCard++; break;
            case 1: BulletCard++; break;
            case 2: barrelCard++; break;
            case 3: weaknessCard++; break;
            case 4: nimblestepsCard++; break;
            case 5: QuickstepCard++; break;
            case 6: fastdraw++; break;
        }

        Debug.Log(cardType + " 번 카드 강화!");

        NoSeeCard();
    }

    // 카드 보이지 않게 하기
    void NoSeeCard()
    {
        for (int i = 0; i < 3; i++)
        {
            SetCardobj[i].SetActive(false);
        }
        Player.SetActive(true);
        gameclick = false;
        Time.timeScale = 1.0f;
    }
}
