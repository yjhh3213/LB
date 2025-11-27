using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;

        // 마우스 방향의 각도 계산
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 좌우 방향에 따라 Y축 회전과 각도 조정
        if (mousePos.x < transform.position.x)
        {
            // 왼쪽을 볼 때 (Y축 180도 회전)
            transform.rotation = Quaternion.Euler(0, 180f, angle);
        }
        else
        {
            // 오른쪽을 볼 때 (Y축 0도)
            transform.rotation = Quaternion.Euler(0, 180f, angle);
        }
    }
}