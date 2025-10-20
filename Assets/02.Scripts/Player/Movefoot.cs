using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movefoot : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        RotateToMouse();
    }

    // 캐릭터가 마우스를 바라보게 하는 코드
    void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float yrot = 0.0f;

        if (mousePos.x < transform.position.x)
        {
            yrot = 180.0f;
        }
        else
        {
            yrot = 0.0f;
        }

        transform.rotation = Quaternion.Euler(0, yrot, 0);
    }
}
