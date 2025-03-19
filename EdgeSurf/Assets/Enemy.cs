using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f; // ���˵��ƶ��ٶ�

    public float mapWidth;      // ������ͼ�Ŀ��
    public float mapHeight;     // ������ͼ�ĸ߶�
    public int mapNums;         // ��ͼ����
    public float totalwidth;    // �ܿ��
    public float totalheight;   // �ܸ߶�

    public GameObject player; // ������Ƕ���
    public float patrolRadius = 5f; // Ѳ��·���뾶
    public float visionRadius = 10f; // ���˵���Ұ�뾶
    public Transform[] patrolPoints; // Ѳ��·���ϵĸ���·��
    protected bool chasingPlayer = false; // �Ƿ���׷�����
    private bool hasAttacked = false; // �Ƿ��Ѿ�������
    public Vector2 origin; // ��ʼλ��
    private bool returningToOrigin = false; // �Ƿ����ڷ��س�ʼλ��
    private int currentPatrolIndex = 0; // ��ǰѲ�ߵ������

    void Start()
    {
        origin = transform.position; // ��¼���˵ĳ�ʼλ��
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
            player = GameObject.FindWithTag("Player"); // ȷ��Ѱ�Ҵ���"Player"��ǩ�Ķ���  
        } 

        if (!chasingPlayer && !returningToOrigin) // ���û��׷����ң�Ҳû�ڷ��س�ʼλ�ã�ִ��Ѳ���߼�
        {
            Patrol();
        }
       
        if (!hasAttacked) // �����û�й������������Ұ��Χ
        {
            CheckVision();
        }

        if (returningToOrigin) // ������ڷ���ԭ��
        {
            ReturnToOrigin();
        }
    }
  
    private void OnDrawGizmosSelected()
    {
        // ����Gizmos����ɫ
        Gizmos.color = Color.yellow;

        // ����һ����ʾ��Ұ��Χ��Բ��
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
    protected virtual void Patrol() // ������·��֮��Ѳ��
    {
        if (patrolPoints.Length == 0) return; // û������·��ʱ��ֱ�ӷ���

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // ֱ���ƶ����ˣ����������ڷ�����
        transform.position = newPosition;

        // �����˵��ﵱǰ·��ʱ��ת����һ��·��
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // ����ѭ��·��
        }
    }

    protected virtual void CheckVision() // �������Ƿ������Ұ��Χ
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= visionRadius) // �������Ұ��Χ��
        {
            chasingPlayer = true; // ����׷��״̬
            ChasePlayer(); // ִ��׷���߼�
        }
        else
        {
            chasingPlayer = false; // ����뿪��Ұ��Χʱ��ֹͣ׷��
        }
    }

    protected virtual void ChasePlayer() // ׷�����
    {
        if (chasingPlayer && !hasAttacked)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);

            // ֱ���ƶ����ˣ����������ڷ�����
            transform.position = targetPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // �������ҵ���ײ
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>(); // ��ȡ��ҽű�
        if (player!= null && !player.isdead) // �����Ҵ�����û�й�����
        {
            player.isdead=true; 
            Debug.Log($"�������");

            // ������ֹͣ׷��������ԭ��
            hasAttacked = true;
            chasingPlayer = false;
            returningToOrigin = true; // ���Ϊ����ԭ��״̬
        }
    }

    protected virtual void ReturnToOrigin() // ���س�ʼλ��
    {
        transform.position = Vector2.MoveTowards(transform.position, origin, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, origin) < 0.1f) // ���������ԭ��
        {
            returningToOrigin = false; // ֹͣ����
            //hasAttacked = false; // ���ù���״̬�������ٴι���
        }
    }
}