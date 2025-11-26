using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float smoothing = 5.0f;
    public float minX, maxX, minY, maxY; // 타일맵 경계 정의
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
    }
   
    private void LateUpdate()
    {
        // 플레이어의 위치 가져오기
        Vector3 playerPos = player.position;

        // 카메라의 위치를 타일맵 경계 내로 제한
        float clampedX = Mathf.Clamp(playerPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(playerPos.y, minY, maxY);

        // Z 축은 유지하고, 카메라의 위치를 설정
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
