using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f; // 敌人的移动速度

    public float mapWidth;      // 单个地图的宽度
    public float mapHeight;     // 单个地图的高度
    public int mapNums;         // 地图数量
    public float totalwidth;    // 总宽度
    public float totalheight;   // 总高度

    public GameObject player; // 存放主角对象
    public float patrolRadius = 5f; // 巡逻路径半径
    public float visionRadius = 10f; // 敌人的视野半径
    public Transform[] patrolPoints; // 巡逻路径上的各个路点
    protected bool chasingPlayer = false; // 是否在追击玩家
    private bool hasAttacked = false; // 是否已经攻击过
    public Vector2 origin; // 初始位置
    private bool returningToOrigin = false; // 是否正在返回初始位置
    private int currentPatrolIndex = 0; // 当前巡逻点的索引

    void Start()
    {
        origin = transform.position; // 记录敌人的初始位置
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        mapNums = 3;

        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        mapHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        totalwidth = mapNums * mapWidth;
        totalheight = mapNums * mapHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // 确保寻找带有"Player"标签的对象  
        } 

        if (!chasingPlayer && !returningToOrigin) // 如果没在追击玩家，也没在返回初始位置，执行巡逻逻辑
        {
            Patrol();
        }
       
        if (!hasAttacked) // 如果还没有攻击过，检查视野范围
        {
            CheckVision();
        }

        if (returningToOrigin) // 如果正在返回原点
        {
            ReturnToOrigin();
        }
    }
  
    private void OnDrawGizmosSelected()
    {
        // 设置Gizmos的颜色
        Gizmos.color = Color.yellow;

        // 绘制一个表示视野范围的圆形
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
    protected virtual void Patrol() // 敌人在路点之间巡逻
    {
        if (patrolPoints.Length == 0) return; // 没有设置路点时，直接返回

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // 直接移动敌人，不再限制在房间内
        transform.position = newPosition;

        // 当敌人到达当前路点时，转向下一个路点
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // 依次循环路点
        }
    }

    protected virtual void CheckVision() // 检查玩家是否进入视野范围
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= visionRadius) // 玩家在视野范围内
        {
            chasingPlayer = true; // 进入追击状态
            ChasePlayer(); // 执行追击逻辑
        }
        else
        {
            chasingPlayer = false; // 玩家离开视野范围时，停止追击
        }
    }

    protected virtual void ChasePlayer() // 追击玩家
    {
        if (chasingPlayer && !hasAttacked)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);

            // 直接移动敌人，不再限制在房间内
            transform.position = targetPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // 检测与玩家的碰撞
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>(); // 获取玩家脚本
        if (player!= null && !player.isdead) // 如果玩家存在且没有攻击过
        {
            player.isdead=true; 
            Debug.Log($"玩家死亡");

            // 攻击后停止追击并返回原点
            hasAttacked = true;
            chasingPlayer = false;
            returningToOrigin = true; // 标记为返回原点状态
        }
    }

    protected virtual void ReturnToOrigin() // 返回初始位置
    {
        transform.position = Vector2.MoveTowards(transform.position, origin, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, origin) < 0.1f) // 如果到达了原点
        {
            returningToOrigin = false; // 停止返回
            //hasAttacked = false; // 重置攻击状态，允许再次攻击
        }
    }
}