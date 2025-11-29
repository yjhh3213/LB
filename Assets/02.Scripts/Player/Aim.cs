using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Sprite[] sprite; // 그냥 조준, 장전 (마우스 커서는 따로)
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite[0];
        Cursor.visible = false;

        mainCamera = Camera.main;
        SetBounds();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // 부드럽게 따라가기
        transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime * 20f);
    }

    void SetBounds()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minX = bottomLeft.x - 8f;
        maxX = topRight.x + 8f;
        minY = bottomLeft.y - 12f;
        maxY = topRight.y + 12f;
    }

    public void aim_ch(string image_st) //에임모양 바꾸기
    {
        if (image_st == "마우스") { Cursor.visible = true; spriteRenderer.sprite = null; }
        if (image_st == "일반") { Cursor.visible = false; spriteRenderer.sprite = sprite[0]; }
        if (image_st == "장전") { Cursor.visible = false; spriteRenderer.sprite = sprite[1]; }
    }
}