using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.5f; // �ƶ��ٶ�
    private float speedMultiplier = 1f; // ��ǰ�ٶȳ���

    private Rigidbody2D rb;
    public float blinkDuration = 2f; // ��˸����ʱ��
    public float invincibleDuration = 3f; // �޵�ʱ��
    public float invincibleAlpha = 0.5f; // �޵��ڼ��͸����
    public bool isInvincible = false; // �Ƿ����޵�״̬
    private SpriteRenderer playerRenderer; // ��ҵ���Ⱦ���
    private Color originalColor; // ���ԭʼ��ɫ
    private Collider2D playerCollider; // ��ҵ���ײ��

    public float Hp = 30f;
    public float maxHp = 30f;
   
    private Vector2 lastPosition; // ��¼�����һ֡��λ��
    public float totalDistance; // ��¼��ҵ����ƶ�����
    private float MaxDistance;//����浵

    private bool isMoving = true; // �Ƿ������ƶ�
    private Vector2 moveDirection ; // ��ʼ�ƶ�����Ϊ����

    public float mouseSensitivity = 0.1f; // ���������
    private Vector2 lastMousePosition; // ��һ֡���λ��

    private float mouseXOffset = 0f; // ���ˮƽƫ����
    private float directionThreshold = 50f; // �����л���ֵ
    private float deadZone = 1f; // ����΢С�ƶ����������л�
    public bool isInputEnabled = true; // �Ƿ������������
    private float rotationSmoothSpeed = 10f; // ��תƽ���ٶ�

    public int boostItemCount = 3; // ӵ�еļ��ٵ�������
    public float boostDuration = 2f; // ���ٳ���ʱ��
    public float boostSpeedMultiplier = 2f; // ����ʱ���ٶȳ���
    private bool isBoosting = false; // �Ƿ����ڼ���

    private TrailRenderer trail;  // ������β���

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        originalColor = playerRenderer.material.color; // �������ԭʼ��ɫ
        lastMousePosition = Input.mousePosition; // ��ʼ�����λ��
        lastPosition = rb.position; // ��ʼ�����λ��
        totalDistance = 0f; // �ƶ������ʼ��Ϊ0
        trail = GetComponent<TrailRenderer>(); // ��ȡ Trail Renderer
        trail.enabled = false; // ��ʼʱ������β
        
        LoadMaxDistance(); // ������߷�  
    }

    void Update()
    {
        if (isInputEnabled)
        {
            HandleKeyboardInput();
            HandleMouseInput();
            HandleMovement();
            RotatePlayer(); // �����ƶ�������ת��ɫ
        }
        // ��������ƶ�����
        CalculateDistance();
    }
    // ��������ƶ�����
    private void CalculateDistance()
    {
        Vector2 currentPosition = rb.position; // ��ȡ��ǰλ��

        // ���㵱ǰλ������һ֡λ��֮��ľ���
        float distanceThisFrame = Vector2.Distance(currentPosition, lastPosition);

        // �ۼ��ܾ���
        totalDistance += distanceThisFrame;

        // ������һ֡λ��
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

    // �����µ��ƶ�����
    public void SetMoveDirection(Vector2 newDirection)
    {
        moveDirection = newDirection;
    }
    void HandleKeyboardInput()
    {
        
        // ���� W ��ֹͣ�ƶ�
        if (Input.GetKeyDown(KeyCode.W))
        {
            isMoving = false;
            rb.velocity = Vector2.zero; // ֹͣ�ƶ�
            moveDirection = Vector2.down; // �ָ����³���
            mouseXOffset = 0f; // �������ƫ����
        }

        // ���� S ���ָ��ƶ�
        if (Input.GetKeyDown(KeyCode.S))
        {
            isMoving = true;
            moveDirection = Vector2.down; // �ָ������ƶ�
            mouseXOffset = 0f; // �������ƫ����
        }

        // ���� A ������
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (mouseXOffset > -directionThreshold)
            {
                moveDirection = new Vector2(-0.5f, -1f).normalized; // ��1
                mouseXOffset = -directionThreshold;
            }
            else
            {
                moveDirection = new Vector2(-1f, -1f).normalized; // ��2
                mouseXOffset = -2 * directionThreshold;
            }
            isMoving = true;
        }

        // ���� D ������
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (mouseXOffset < directionThreshold)
            {
                moveDirection = new Vector2(0.5f, -1f).normalized; // ��1
                mouseXOffset = directionThreshold;
            }
            else
            {
                moveDirection = new Vector2(1f, -1f).normalized; // ��2
                mouseXOffset = 2 * directionThreshold;
            }
            isMoving = true;
        }
    }

    void HandleMouseInput()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        float mouseDeltaX = (currentMousePosition.x - lastMousePosition.x) * mouseSensitivity; // �������ˮƽƫ����
        float mouseDeltaY = (currentMousePosition.y - lastMousePosition.y) * mouseSensitivity; // ������괹ֱƫ����
        lastMousePosition = currentMousePosition;

        // �ۼ����ˮƽƫ����
        mouseXOffset += mouseDeltaX;

        // �������ƫ�����ķ�Χ
        mouseXOffset = Mathf.Clamp(mouseXOffset, -2 * directionThreshold, 2 * directionThreshold);

        // �����������ƶ�ֹͣ�ƶ�
        if (mouseDeltaY > 2f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero; // ֹͣ�ƶ�
            moveDirection = Vector2.down; // �ָ����³���
            mouseXOffset = 0f; // �������ƫ����
            return;
        }

        // �����������ƶ��ָ��ƶ�
        if (mouseDeltaY < -deadZone)
        {
            isMoving = true;
            moveDirection = Vector2.down; // �ָ������ƶ�
            mouseXOffset = 0f; // �������ƫ����
        }

        // �������ˮƽƫ�����л������������
        if (mouseXOffset < -1.5f * directionThreshold)
        {
            moveDirection = new Vector2(-1f, -1f).normalized; // ��2
        }
        else if (mouseXOffset < -0.5f * directionThreshold)
        {
            moveDirection = new Vector2(-0.5f, -1f).normalized; // ��1
        }
        else if (mouseXOffset > 1.5f * directionThreshold)
        {
            moveDirection = new Vector2(1f, -1f).normalized; // ��2
        }
        else if (mouseXOffset > 0.5f * directionThreshold)
        {
            moveDirection = new Vector2(0.5f, -1f).normalized; // ��1
        }
        else
        {
            moveDirection = Vector2.down; // ���·�
        }
    }

    void HandleMovement()
    {
        if (isMoving)
        {
            // �ж��Ƿ���Լ���  
            if (boostItemCount > 0 && Input.GetKeyDown(KeyCode.E))
            {
                boostItemCount--; // ����һ������
                StartCoroutine(BoostSpeed()); // ��������Э��
            }

            // �����Ƿ����ڼ����������ٶ�
            float currentSpeedMultiplier = isBoosting ? boostSpeedMultiplier : speedMultiplier;
            rb.velocity = moveDirection * walkSpeed * currentSpeedMultiplier; // �����ƶ�������ƶ�
        }
    }


    void RotatePlayer()
    {
        if (moveDirection != Vector2.zero)
        {
            // �����ƶ�����ĽǶ�
            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90;
            // ʹ�ò�ֵƽ����ת
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone") && !isInvincible)
        {
            Hp -= 10f;
            rb.velocity = Vector2.zero; // ֹͣ�ƶ�
            isInputEnabled = false; // �����������
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
            // �񵽵��ߺ󴥷��޵�״̬  
            StartCoroutine(ActivateInvincibility());
        }
        if (other.gameObject.CompareTag("Board") && !isInvincible)
        {
            // �����󴥷��޵�״̬  
            StartCoroutine(ActivateInvincibility());
            //����
            StartCoroutine(BoostSpeed());

        }
       
    }
    

      //����
      IEnumerator BoostSpeed()
    {
        isBoosting = true; // ��ʼ����  
        float originalSpeed = walkSpeed; // ����ԭʼ�ٶ�  
        walkSpeed *= boostSpeedMultiplier; // ���� 
        trail.enabled = true; // ������β
        yield return new WaitForSeconds(boostDuration); // �ȴ����ٳ���ʱ��  
        trail.enabled = false; // �ر���β
        walkSpeed = originalSpeed; // �ָ�ԭʼ�ٶ�  
        isBoosting = false; // ��������  
    }
    //��˸���޵�
    IEnumerator BlinkAndInvincible()
    {
        isInvincible = true;

        // ��˸Ч��
        float endTime = Time.time + blinkDuration;
        while (Time.time < endTime)
        {
            playerRenderer.enabled = !playerRenderer.enabled; // �л��ɼ���
            yield return new WaitForSeconds(0.1f); // ������˸�ٶ�
        }

        // ȷ����˸��������ҿɼ�
        playerRenderer.enabled = true;
        // �����������
        isInputEnabled = true; 
        // �����޵��ڼ��͸����
        Color invincibleColor = originalColor;
        invincibleColor.a = invincibleAlpha;
        playerRenderer.color = invincibleColor;

        // ������ײ��
        playerCollider.enabled = false;

        // �ȴ��޵�ʱ��
        yield return new WaitForSeconds(invincibleDuration);

        // �ָ�ԭʼ��ɫ
        playerRenderer.color = originalColor;

        // ������ײ��
        playerCollider.enabled = true;

        // �����޵�״̬
        isInvincible = false;
    }
    //�޵�
    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // �����޵��ڼ��͸����  
        Color invincibleColor = originalColor;
        invincibleColor.a = invincibleAlpha; // ����͸����  
        playerRenderer.color = invincibleColor;

        // ������ײ��  
        playerCollider.enabled = false;

        // �ȴ�һ��ʱ�䣨���Ե�������ʱ�䣩  
        yield return new WaitForSeconds(invincibleDuration);

        // �ָ�ԭʼ��ɫ  
        playerRenderer.color = originalColor;

        // ������ײ��  
        playerCollider.enabled = true;

        // �����޵�״̬  
        isInvincible = false;
    }

    public void SetSpeedMultiplier(float factor)
    {
        speedMultiplier = factor; // ���µ�ǰ�ٶȳ���  
    }

    // �����ٶȳ����ķ���  
    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f; // �ָ�Ϊ�����ٶ�  
    }
}