using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public Vector2[] bounceDirections= new Vector2[]
{
    new Vector2(0.5f, -1f).normalized, // ��1
    new Vector2(-0.5f, -1f).normalized // ��1
}; // ���巴�����򣨿��Ը�����ҵ�ǰ����ѡ��

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ������������
        {
            // ��ȡ��ҵ� PlayerController ���
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // ��ȡ��ҵ�ǰ����
                Vector2 currentDirection = playerController.GetMoveDirection();

                // ���ݵ�ǰ������㷴������
                Vector2 bounceDirection = CalculateBounceDirection(currentDirection);

                // ������ҵ��ƶ�����
                playerController.SetMoveDirection(bounceDirection);

                Debug.Log("��Ҵ����� Bounce����ǰ����: " + currentDirection + "����������: " + bounceDirection);
            }
        }
    }

    // ������ҵ�ǰ������㷴������
    private Vector2 CalculateBounceDirection(Vector2 currentDirection)
    {
        // Ĭ�Ϸ�������Ϊ��ǰ������෴����
        Vector2 bounceDirection = -currentDirection;

        // ������Զ���ķ����������飬���Ը��ݵ�ǰ����ѡ��
        if (bounceDirections != null && bounceDirections.Length > 0)
        {
            // ������Ը�����Ҫʵ�ָ����ӵ��߼�
            // ���磺������ҵ�ǰ����ѡ����ӽ��ķ�������
            bounceDirection = bounceDirections[0]; // ��ʱʹ�õ�һ������
        }

        return bounceDirection.normalized; // ���ص�λ����
    }
}
