using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceWave : MonoBehaviour
{
    public float speedReductionFactor = 0.5f; // 减速因子  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查是否与玩家碰撞  
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetSpeedMultiplier(speedReductionFactor); // 设置减速  
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 检查是否与玩家碰撞结束  
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ResetSpeedMultiplier(); // 恢复速度  
            }
        }
    }
}