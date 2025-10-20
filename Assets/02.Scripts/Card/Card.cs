using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CountTimer ct;
    public GameObject[] SetCardobj;     // 카드오브젝트
    public Sprite[] CardImage;           // 카드 이미지
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

    int Sect1 = 0;      // SetCardobj0번째
    int Sect2 = 0;      // SetCardobj1번째
    int Sect3 = 0;      // SetCardobj2번째
    void cardbuff()
    {
        Time.timeScale = 0.0f;
        if(CardImage != null && SetCardobj != null)
        {
            for(int i = 0; i < 3; i++)
            {
                int num = Random.Range(1, 7);
                Image cardDisplay = SetCardobj[i].GetComponent<Image>();

                if(cardDisplay != null)
                {
                    cardDisplay.sprite = CardImage[num];
                }

                if(i == 0)
                {
                    Sect1 = num;
                }
                else if (i == 1)
                {
                    Sect2 = num;
                }
                else if (i == 2)
                {
                    Sect3 = num;
                }
            }
        }
    }
}
