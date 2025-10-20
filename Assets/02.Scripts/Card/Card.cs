using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    CountTimer ct;
    public GameObject[] SetCardobj;     // ī�������Ʈ
    public Sprite[] CardImage;           // ī�� �̹���
    public int ShotGunCard = 0;         // ���ǰ���
    public int BulletCard = 0;          // �Ѿ˰���
    public int barrelCard = 0;          // �ѿ�����
    public int weaknessCard = 0;        // ��������
    public int nimblestepsCard = 0;     // ����Ѱ���
    public int QuickstepCard = 0;       // �� ����
    public int fastdraw = 0;            // ���� ����

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

    int Sect1 = 0;      // SetCardobj0��°
    int Sect2 = 0;      // SetCardobj1��°
    int Sect3 = 0;      // SetCardobj2��°
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
