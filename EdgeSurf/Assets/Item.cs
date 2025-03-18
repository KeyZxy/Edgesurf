using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject mainCam;   // �������
    public float mapWidth;      // ������ͼ�Ŀ��
    public float mapHeight;     // ������ͼ�ĸ߶�
    public float totalwidth;    // �ܿ��
    public float totalheight;   // �ܸ߶�
    public GameObject[]ItemPrefabs; // �ϰ���Ԥ��������
    public int mapNums;         // ��ͼ����
    public int min = 2;         // ��С��������
    public int max = 4;         // �����������
    public float currenttime = 0;
    private List<GameObject> activeObjects = new List<GameObject>(); // ��ǰ����ϰ����б�
    private bool isSpawn = false; // �Ƿ���ͣ���ɺ�����
    void Start()
    {
        mapNums = 3;
        mainCam = GameObject.FindWithTag("MainCamera");
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        mapHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        totalwidth = mapNums * mapWidth;
        totalheight = mapNums * mapHeight;
    }

    // Update is called once per frame
    void Update()
    {
        currenttime += Time.deltaTime;
        if (currenttime > 3f&&!isSpawn)
        {
            isSpawn = true;
            SpawnObjects(); 
        }
    }
    void SpawnObjects()
    {
        int obstacleCount = Random.Range(min, max);
        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject obstacleToSpawn =ItemPrefabs[Random.Range(0, ItemPrefabs.Length)];
            Vector2 spawnPosition = new Vector2(
                Random.Range(-mapWidth / 2, mapWidth / 2), // �ڵ�ͼ�ֲ���Χ������
                Random.Range(-mapHeight / 2, mapHeight / 2)
            );
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.TransformPoint(spawnPosition), Quaternion.identity);
            obstacle.transform.parent = transform; // ���ϰ�������Ϊ��ͼ���Ӷ���
            activeObjects.Add(obstacle); // �����ɵ��ϰ�����ӵ���б�
        }

    }
}
