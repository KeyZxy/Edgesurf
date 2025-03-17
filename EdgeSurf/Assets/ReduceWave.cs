using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceWave : MonoBehaviour
{
    public float speedReductionFactor = 0.5f; // ��������  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����Ƿ��������ײ  
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetSpeedMultiplier(speedReductionFactor); // ���ü���  
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ����Ƿ��������ײ����  
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ResetSpeedMultiplier(); // �ָ��ٶ�  
            }
        }
    }
}