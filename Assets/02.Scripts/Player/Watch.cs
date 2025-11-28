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

        // Z축 회전 적용
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 좌우 방향에 따라 Y축 스케일 조정 (상하 반전)
        if (mousePos.x < transform.position.x)
        {
            // 왼쪽을 볼 때 - Y축 스케일 반전
            transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
        else
        {
            // 오른쪽을 볼 때 - Y축 스케일 정상
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }
    }
}