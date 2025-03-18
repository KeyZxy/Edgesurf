using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.5f; // 移动速度
    private float speedMultiplier = 1f; // 当前速度乘数

    private Rigidbody2D rb;
    public float blinkDuration = 2f; // 闪烁持续时间
    public float invincibleDuration = 3f; // 无敌时间
    public float invincibleAlpha = 0.5f; // 无敌期间的透明度
    public bool isInvincible = false; // 是否处于无敌状态
    private SpriteRenderer playerRenderer; // 玩家的渲染组件
    private Color originalColor; // 玩家原始颜色
    private Collider2D playerCollider; // 玩家的碰撞器

    public float Hp = 30f;
    public float maxHp = 30f;
   
    private Vector2 lastPosition; // 记录玩家上一帧的位置
    public float totalDistance; // 记录玩家的总移动距离
    private float MaxDistance;//距离存档

    private bool isMoving = true; // 是否正在移动
    private Vector2 moveDirection ; // 初始移动方向为向下

    public float mouseSensitivity = 0.1f; // 鼠标灵敏度
    private Vector2 lastMousePosition; // 上一帧鼠标位置

    private float mouseXOffset = 0f; // 鼠标水平偏移量
    private float directionThreshold = 50f; // 方向切换阈值
    private float deadZone = 1f; // 避免微小移动触发方向切换
    public bool isInputEnabled = true; // 是否允许玩家输入
    private float rotationSmoothSpeed = 10f; // 旋转平滑速度

    public int boostItemCount = 3; // 拥有的加速道具数量
    public float boostDuration = 2f; // 加速持续时间
    public float boostSpeedMultiplier = 2f; // 加速时的速度乘数
    private bool isBoosting = false; // 是否正在加速

    private TrailRenderer trail;  // 引用拖尾组件

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        originalColor = playerRenderer.material.color; // 保存玩家原始颜色
        lastMousePosition = Input.mousePosition; // 初始化鼠标位置
        lastPosition = rb.position; // 初始化玩家位置
        totalDistance = 0f; // 移动距离初始化为0
        trail = GetComponent<TrailRenderer>(); // 获取 Trail Renderer
        trail.enabled = false; // 初始时禁用拖尾
        
        LoadMaxDistance(); // 加载最高分  
    }

    void Update()
    {
        if (isInputEnabled)
        {
            HandleKeyboardInput();
            HandleMouseInput();
            HandleMovement();
            RotatePlayer(); // 根据移动方向旋转角色
        }
        // 计算玩家移动距离
        CalculateDistance();
    }
    // 计算玩家移动距离
    private void CalculateDistance()
    {
        Vector2 currentPosition = rb.position; // 获取当前位置

        // 计算当前位置与上一帧位置之间的距离
        float distanceThisFrame = Vector2.Distance(currentPosition, lastPosition);

        // 累加总距离
        totalDistance += distanceThisFrame;

        // 更新上一帧位置
        lastPosition = currentPosition;
    }

    private void LoadMaxDistance()
    {
        MaxDistance = PlayerPrefs.GetFloat("newMax", 0f);
        Debug.Log("Loaded Max Distance: " + MaxDistance);
    }
    public float GetScore()
    {
        return totalDistance;
    }
    public void SetmaxDistance(float maxdis)
    {
        MaxDistance=maxdis;
    }
    public float GetmaxDistance()
    {
        return MaxDistance;
    }
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // 设置新的移动方向
    public void SetMoveDirection(Vector2 newDirection)
    {
        moveDirection = newDirection;
    }
    void HandleKeyboardInput()
    {
        
        // 按下 W 键停止移动
        if (Input.GetKeyDown(KeyCode.W))
        {
            isMoving = false;
            rb.velocity = Vector2.zero; // 停止移动
            moveDirection = Vector2.down; // 恢复向下朝向
            mouseXOffset = 0f; // 重置鼠标偏移量
        }

        // 按下 S 键恢复移动
        if (Input.GetKeyDown(KeyCode.S))
        {
            isMoving = true;
            moveDirection = Vector2.down; // 恢复向下移动
            mouseXOffset = 0f; // 重置鼠标偏移量
        }

        // 处理 A 键按下
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (mouseXOffset > -directionThreshold)
            {
                moveDirection = new Vector2(-0.5f, -1f).normalized; // 左1
                mouseXOffset = -directionThreshold;
            }
            else
            {
                moveDirection = new Vector2(-1f, -1f).normalized; // 左2
                mouseXOffset = -2 * directionThreshold;
            }
            isMoving = true;
        }

        // 处理 D 键按下
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (mouseXOffset < directionThreshold)
            {
                moveDirection = new Vector2(0.5f, -1f).normalized; // 右1
                mouseXOffset = directionThreshold;
            }
            else
            {
                moveDirection = new Vector2(1f, -1f).normalized; // 右2
                mouseXOffset = 2 * directionThreshold;
            }
            isMoving = true;
        }
    }

    void HandleMouseInput()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        float mouseDeltaX = (currentMousePosition.x - lastMousePosition.x) * mouseSensitivity; // 计算鼠标水平偏移量
        float mouseDeltaY = (currentMousePosition.y - lastMousePosition.y) * mouseSensitivity; // 计算鼠标垂直偏移量
        lastMousePosition = currentMousePosition;

        // 累加鼠标水平偏移量
        mouseXOffset += mouseDeltaX;

        // 限制鼠标偏移量的范围
        mouseXOffset = Mathf.Clamp(mouseXOffset, -2 * directionThreshold, 2 * directionThreshold);

        // 如果鼠标往上移动停止移动
        if (mouseDeltaY > 2f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero; // 停止移动
            moveDirection = Vector2.down; // 恢复向下朝向
            mouseXOffset = 0f; // 重置鼠标偏移量
            return;
        }

        // 如果鼠标往下移动恢复移动
        if (mouseDeltaY < -deadZone)
        {
            isMoving = true;
            moveDirection = Vector2.down; // 恢复向下移动
            mouseXOffset = 0f; // 重置鼠标偏移量
        }

        // 根据鼠标水平偏移量切换方向（五个方向）
        if (mouseXOffset < -1.5f * directionThreshold)
        {
            moveDirection = new Vector2(-1f, -1f).normalized; // 左2
        }
        else if (mouseXOffset < -0.5f * directionThreshold)
        {
            moveDirection = new Vector2(-0.5f, -1f).normalized; // 左1
        }
        else if (mouseXOffset > 1.5f * directionThreshold)
        {
            moveDirection = new Vector2(1f, -1f).normalized; // 右2
        }
        else if (mouseXOffset > 0.5f * directionThreshold)
        {
            moveDirection = new Vector2(0.5f, -1f).normalized; // 右1
        }
        else
        {
            moveDirection = Vector2.down; // 正下方
        }
    }

    void HandleMovement()
    {
        if (isMoving)
        {
            // 判断是否可以加速  
            if (boostItemCount > 0 && Input.GetKeyDown(KeyCode.E))
            {
                boostItemCount--; // 消耗一个道具
                StartCoroutine(BoostSpeed()); // 启动加速协程
            }

            // 根据是否正在加速来设置速度
            float currentSpeedMultiplier = isBoosting ? boostSpeedMultiplier : speedMultiplier;
            rb.velocity = moveDirection * walkSpeed * currentSpeedMultiplier; // 正常移动或加速移动
        }
    }


    void RotatePlayer()
    {
        if (moveDirection != Vector2.zero)
        {
            // 计算移动方向的角度
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90;
            // 使用插值平滑旋转
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") && !isInvincible)
        {
            Hp -= 10f;
            rb.velocity = Vector2.zero; // 停止移动
            isInputEnabled = false; // 禁用玩家输入
            StartCoroutine(BlinkAndInvincible());
        }
        if (collision.gameObject.CompareTag("Bounce") )
        {
            //
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            Destroy(other.gameObject);
            boostItemCount++;
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(other.gameObject);
            // 捡到道具后触发无敌状态  
            StartCoroutine(ActivateInvincibility());
        }
        if (other.gameObject.CompareTag("Board") && !isInvincible)
        {
            // 触碰后触发无敌状态  
            StartCoroutine(ActivateInvincibility());
            //加速
            StartCoroutine(BoostSpeed());

        }
       
    }
    

      //加速
      IEnumerator BoostSpeed()
    {
        isBoosting = true; // 开始加速  
        float originalSpeed = walkSpeed; // 保存原始速度  
        walkSpeed *= boostSpeedMultiplier; // 加速 
        trail.enabled = true; // 启用拖尾
        yield return new WaitForSeconds(boostDuration); // 等待加速持续时间  
        trail.enabled = false; // 关闭拖尾
        walkSpeed = originalSpeed; // 恢复原始速度  
        isBoosting = false; // 结束加速  
    }
    //闪烁接无敌
    IEnumerator BlinkAndInvincible()
    {
        isInvincible = true;

        // 闪烁效果
        float endTime = Time.time + blinkDuration;
        while (Time.time < endTime)
        {
            playerRenderer.enabled = !playerRenderer.enabled; // 切换可见性
            yield return new WaitForSeconds(0.1f); // 控制闪烁速度
        }

        // 确保闪烁结束后玩家可见
        playerRenderer.enabled = true;
        // 启用玩家输入
        isInputEnabled = true; 
        // 设置无敌期间的透明度
        Color invincibleColor = originalColor;
        invincibleColor.a = invincibleAlpha;
        playerRenderer.color = invincibleColor;

        // 禁用碰撞器
        playerCollider.enabled = false;

        // 等待无敌时间
        yield return new WaitForSeconds(invincibleDuration);

        // 恢复原始颜色
        playerRenderer.color = originalColor;

        // 启用碰撞器
        playerCollider.enabled = true;

        // 结束无敌状态
        isInvincible = false;
    }
    //无敌
    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // 设置无敌期间的透明度  
        Color invincibleColor = originalColor;
        invincibleColor.a = invincibleAlpha; // 设置透明度  
        playerRenderer.color = invincibleColor;

        // 禁用碰撞器  
        playerCollider.enabled = false;

        // 等待一段时间（可以调整持续时间）  
        yield return new WaitForSeconds(invincibleDuration);

        // 恢复原始颜色  
        playerRenderer.color = originalColor;

        // 启用碰撞器  
        playerCollider.enabled = true;

        // 结束无敌状态  
        isInvincible = false;
    }

    public void SetSpeedMultiplier(float factor)
    {
        speedMultiplier = factor; // 更新当前速度乘数  
    }

    // 重置速度乘数的方法  
    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f; // 恢复为正常速度  
    }
}