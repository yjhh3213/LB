using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombieSkin : MonoBehaviour
{
    public int ty; //1,2,3,4

    public SpriteRenderer Body;
    public SpriteRenderer Hands;

    public Animator tlcp_an;
    public Animator Feet_an;

    public Sprite[] Body_spr;
    public Sprite[] Hands_spr;

    public RuntimeAnimatorController[] tlcp_Controller;
    public RuntimeAnimatorController[] Feet_Controller;

    void Start()
    {
        ty=Random.Range(0, 4);
        Body.sprite = Body_spr[ty];
        Hands.sprite = Hands_spr[ty];

        tlcp_an.runtimeAnimatorController = tlcp_Controller[ty];
        Feet_an.runtimeAnimatorController = Feet_Controller[ty];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
