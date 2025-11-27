using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Sprite[] sprite; // 그냥 조준, 장전 (마우스 커서는 따로)
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite[0];
        Cursor.visible = false;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    public void aim_ch(string image_st) //에임모양 바꾸기
    {
        if (image_st == "마우스") { Cursor.visible = true; spriteRenderer.sprite = null; }
        if (image_st == "일반") { Cursor.visible = false; spriteRenderer.sprite = sprite[0]; }
        if (image_st == "장전") { Cursor.visible = false; spriteRenderer.sprite = sprite[1]; }
    }
}
