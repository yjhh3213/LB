using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float smoothing = 5.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
    }
}
