using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //public CountTimer ct;

    public static Card Instance;

    private float card_delay = 1f;      // 카드를 고른 순간 카드 시간 부과
    private float card_cu_delay = 0f;   // 카드를 고를땐 0이여야 고를 수 있음

    [Header("GameObjet")]
    public GameObject[] SetCardobj;     // 카드오브젝트
    public GameObject[] ButtonBGCobj;   // 카드테두리오브젝트
    public Image[] ButtonBGC;           // 버튼 뒤에 색깔 테두리
    public GameObject Player;           // 플레이어
    public Image[] CardColor;           // 카드 테두리 색깔
    public GameObject[] CardColorobj;   // 카드 UI On/Off
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

    // 중복된 카드가 안나오게 하는 변수
    bool CheckShotCard = false;
    bool CheckBulletCard = false;
    bool CheckbarrelCard = false;
    bool CheckweaknessCard = false;
    bool ChecknimblestepsCard = false;
    bool CheckQuickstepCard = false;
    bool Checkfastdraw = false;

    string CardName;
    string CardDetail;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("딜레이 :" + card_cu_delay);
        if (card_cu_delay > 0) { card_cu_delay -= Time.unscaledDeltaTime; }
        ChangeCardColor();
    }

    private void Awake()
    {
        Instance = this;
    }

    void ChangeCardColor()
    {
        // 해당 카드마다 해당 하는 레벨의 색깔 지정하기
        if (ShotGunCard >= 3) CardColor[0].color = new Color(200, 0, 248);
        else if (ShotGunCard == 2) CardColor[0].color = new Color(0, 144, 248);
        else if (ShotGunCard == 1) CardColor[0].color = new Color(152, 248, 152);
        else { CardColorobj[0].SetActive(false); }

        if (BulletCard >= 3) CardColor[1].color = new Color(200, 0, 248);
        else if (BulletCard == 2) CardColor[1].color = new Color(0, 144, 248);
        else if (BulletCard == 1) CardColor[1].color = new Color(152, 248, 152);
        else { CardColorobj[1].SetActive(false); }

        if (barrelCard >= 3) CardColor[2].color = new Color(200, 0, 248);
        else if (barrelCard == 2) CardColor[2].color = new Color(0, 144, 248);
        else if (barrelCard == 1) CardColor[2].color = new Color(152, 248, 152);
        else { CardColorobj[2].SetActive(false); }

        if (weaknessCard >= 3) CardColor[3].color = new Color(200, 0, 248);
        else if (weaknessCard == 2) CardColor[3].color = new Color(0, 144, 248);
        else if (weaknessCard == 1) CardColor[3].color = new Color(152, 248, 152);
        else { CardColorobj[3].SetActive(false); }

        if (nimblestepsCard >= 3) CardColor[4].color = new Color(200, 0, 248);
        else if (nimblestepsCard == 2) CardColor[4].color = new Color(0, 144, 248);
        else if (nimblestepsCard == 1) CardColor[4].color = new Color(152, 248, 152);
        else { CardColorobj[4].SetActive(false); }

        if (QuickstepCard >= 3) CardColor[5].color = new Color(200, 0, 248);
        else if (QuickstepCard == 2) CardColor[5].color = new Color(0, 144, 248);
        else if (QuickstepCard == 1) CardColor[5].color = new Color(152, 248, 152);
        else { CardColorobj[5].SetActive(false); }

        if (fastdraw >= 3) CardColor[6].color = new Color(200, 0, 248);
        else if (fastdraw == 2) CardColor[6].color = new Color(0, 144, 248);
        else if (fastdraw == 1) CardColor[6].color = new Color(152, 248, 152);
        else { CardColorobj[6].SetActive(false); }
    }

  
    // 카드의 이름, 내용 소개
    void CardNameAndDetail(int num, int i)
    {
        switch (num)
        {
            case 0:
                if (ShotGunCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "샷건개조 Level 3"; CardDetail = "기존 투사체에서 +4"; }
                else if (ShotGunCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248);  CardName = "샷건개조 Level 2"; CardDetail = "기존 투사체에서 +2"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "샷건개조 Level 1"; CardDetail = "기존 투사체에서 +1"; }
                break;
            case 1:
                if (BulletCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "총알개조 Level 2"; CardDetail = "적 두 명 관통"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "총알개조 Level 1"; CardDetail = "적 한 명 관통"; }
                break;
            case 2:
                if (barrelCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "총열개조 Level 3"; CardDetail = "탄 퍼짐 60% 감소"; }
                else if (barrelCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "총열개조 Level 2"; CardDetail = "탄 퍼짐 40% 감소"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "총열개조 Level 1"; CardDetail = "탄 퍼짐 20% 감소"; }
                break;
            case 3:
                if (weaknessCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "약점포착 Level 3"; CardDetail = "총알 데미지 50% 증가\n공격속도 20% 증가"; }
                else if (weaknessCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "약점포착 Level 2"; CardDetail = "총알 데미지 40% 증가"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "약점포착 Level 1"; CardDetail = "총알 데미지 20% 증가"; }
                break;
            case 4:
                if (nimblestepsCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "기민한 걸음 Level 3"; CardDetail = "이동속도 +3 증가"; }
                else if (nimblestepsCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "기민한 걸음 Level 2"; CardDetail = "이동속도 +2 증가"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "기민한 걸음 Level 1"; CardDetail = "이동속도 +1 증가"; }
                break;
            case 5:
                if (QuickstepCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "퀵 스텝 Level 3"; CardDetail = "대쉬 쿨타임 3초 감소"; }
                else if (QuickstepCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "퀵 스텝 Level 2"; CardDetail = "대쉬 쿨타임 2초 감소"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "퀵 스텝 Level 1"; CardDetail = "대쉬 쿨타임 1초 감소"; }
                break;
            case 6:
                if (fastdraw == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "빠른 장전 Level 3"; CardDetail = "즉시 재장전"; }
                else if (fastdraw == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "빠른 장전 Level 2"; CardDetail = "재장전시 입력해야 할 수 2개 감소"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "빠른 장전 Level 1"; CardDetail = "재장전시 입력해야 할 수 1개 감소"; }
                break;
            default:
                CardName = "";
                CardDetail = "";
                break;
            
                
        }
    }

    /*void CheckCard(int num, int i)
    {
        switch (num)
        {
            case 0:
                if(CheckShotCard)
                break;
            case 1:
                if (BulletCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "총알개조 Level 2"; CardDetail = "적 두 명 관통"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "총알개조 Level 1"; CardDetail = "적 한 명 관통"; }
                break;
            case 2:
                if (barrelCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "총열개조 Level 3"; CardDetail = "탄 퍼짐 60% 감소"; }
                else if (barrelCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "총열개조 Level 2"; CardDetail = "탄 퍼짐 40% 감소"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "총열개조 Level 1"; CardDetail = "탄 퍼짐 20% 감소"; }
                break;
            case 3:
                if (weaknessCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "약점포착 Level 3"; CardDetail = "총알 데미지 50% 증가\n공격속도 20% 증가"; }
                else if (weaknessCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "약점포착 Level 2"; CardDetail = "총알 데미지 40% 증가"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "약점포착 Level 1"; CardDetail = "총알 데미지 20% 증가"; }
                break;
            case 4:
                if (nimblestepsCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "기민한 걸음 Level 3"; CardDetail = "이동속도 +3 증가"; }
                else if (nimblestepsCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "기민한 걸음 Level 2"; CardDetail = "이동속도 +2 증가"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "기민한 걸음 Level 1"; CardDetail = "이동속도 +1 증가"; }
                break;
            case 5:
                if (QuickstepCard == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "퀵 스텝 Level 3"; CardDetail = "대쉬 쿨타임 3초 감소"; }
                else if (QuickstepCard == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "퀵 스텝 Level 2"; CardDetail = "대쉬 쿨타임 2초 감소"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "퀵 스텝 Level 1"; CardDetail = "대쉬 쿨타임 1초 감소"; }
                break;
            case 6:
                if (fastdraw == 2) { ButtonBGC[i].color = new Color(200, 0, 248); CardName = "빠른 장전 Level 3"; CardDetail = "즉시 재장전"; }
                else if (fastdraw == 1) { ButtonBGC[i].color = new Color(0, 144, 248); CardName = "빠른 장전 Level 2"; CardDetail = "재장전시 입력해야 할 수 2개 감소"; }
                else { ButtonBGC[i].color = new Color(152, 248, 152); CardName = "빠른 장전 Level 1"; CardDetail = "재장전시 입력해야 할 수 1개 감소"; }
                break;
            default:
                CardName = "";
                CardDetail = "";
                break;
        }
    }*/

    int Sect1 = 0;      // SetCardobj0번째
    int Sect2 = 0;      // SetCardobj1번째
    int Sect3 = 0;      // SetCardobj2번째

    List<int> Arraynum;
    public void cardbuff()
    {
        Time.timeScale = 0.0f;
        card_cu_delay = card_delay; //딜레이 넣기
        FindObjectOfType<Aim>().aim_ch("마우스"); //에임 모양 변경
        gameclick = true;

        Arraynum = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

        if (ShotGunCard >= 3) Arraynum.Remove(0);
        if (BulletCard >= 2) Arraynum.Remove(1);
        if (barrelCard >= 3) Arraynum.Remove(2);
        if (weaknessCard >= 3) Arraynum.Remove(3);
        if (nimblestepsCard >= 3) Arraynum.Remove(4);
        if (QuickstepCard >= 3) Arraynum.Remove(5);
        if (fastdraw >= 3) Arraynum.Remove(6);

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
                CardNameAndDetail(num, i);
                SetCardobj[i].transform.GetChild(0).GetComponent<Text>().text = CardName;
                SetCardobj[i].transform.GetChild(1).GetComponent<Text>().text = CardDetail;

                if (i == 0)
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
                ButtonBGCobj[i].SetActive(true);
            }
        }
    }

    // 카드를 눌렀을 때 해당하는 카드의 능력치 올리기
    public void SelectCard(int index)
    {
        if (card_cu_delay <= 0)
        {
            int cardType = 0;

            if (index == 0) cardType = Sect1;
            else if (index == 1) cardType = Sect2;
            else if (index == 2) cardType = Sect3;

            switch (cardType)
            {
                case 0: ShotGunCard++; CardColorobj[0].SetActive(true); break;
                case 1: BulletCard++; CardColorobj[1].SetActive(true); break;
                case 2: barrelCard++; CardColorobj[2].SetActive(true); break;
                case 3: weaknessCard++; CardColorobj[3].SetActive(true); break;
                case 4: nimblestepsCard++; CardColorobj[4].SetActive(true); break;
                case 5: QuickstepCard++; CardColorobj[5].SetActive(true); break;
                case 6: fastdraw++; CardColorobj[6].SetActive(true); break;
            }

            Debug.Log(cardType + " 번 카드 강화!");

            NoSeeCard();
        }
    }

    // 카드 보이지 않게 하기
    void NoSeeCard()
    {
        for (int i = 0; i < 3; i++)
        {
            SetCardobj[i].SetActive(false);
            ButtonBGCobj[i].SetActive(false);
        }
        card_cu_delay = card_delay; //딜레이 넣기
        FindObjectOfType<Aim>().aim_ch("일반"); //에임 모양 변경
        gameclick = false;
        Time.timeScale = 1.0f;
    }
}
