using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public Vector2[] bounceDirections= new Vector2[]
{
    new Vector2(0.5f, -1f).normalized, // 右1
    new Vector2(-0.5f, -1f).normalized // 左1
}; // 定义反弹方向（可以根据玩家当前方向选择）

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 如果触碰到玩家
        {
            // 获取玩家的 PlayerController 组件
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // 获取玩家当前方向
                Vector2 currentDirection = playerController.GetMoveDirection();

                // 根据当前方向计算反弹方向
                Vector2 bounceDirection = CalculateBounceDirection(currentDirection);

                // 更新玩家的移动方向
                playerController.SetMoveDirection(bounceDirection);

                Debug.Log("玩家触碰到 Bounce，当前方向: " + currentDirection + "，反弹方向: " + bounceDirection);
            }
        }
    }

    // 根据玩家当前方向计算反弹方向
    private Vector2 CalculateBounceDirection(Vector2 currentDirection)
    {
        // 默认反弹方向为当前方向的相反方向
        Vector2 bounceDirection = -currentDirection;

        // 如果有自定义的反弹方向数组，可以根据当前方向选择
        if (bounceDirections != null && bounceDirections.Length > 0)
        {
            // 这里可以根据需要实现更复杂的逻辑
            // 例如：根据玩家当前方向选择最接近的反弹方向
            bounceDirection = bounceDirections[0]; // 暂时使用第一个方向
        }

        return bounceDirection.normalized; // 返回单位向量
    }
}
