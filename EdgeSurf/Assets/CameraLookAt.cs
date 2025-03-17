using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{

    public Transform target;  // 要跟随的目标  
    public Vector3 offset;    // 相机与目标之间的偏移  
    public float smoothSpeed = 0.125f; // 平滑跟随的速度  
    private void Update()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            // 计算目标位置加上偏移  
            Vector3 desiredPosition = target.position + offset;

            // 平滑移动相机到目标位置  
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 确保相机没有旋转（保持在2D视图）  
            transform.rotation = Quaternion.identity;
        }
    }
}
