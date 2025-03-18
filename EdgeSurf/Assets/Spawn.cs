using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject mainCam;   // �������
    public float mapWidth;      // ������ͼ�Ŀ��
    public float mapHeight;     // ������ͼ�ĸ߶�
    public float totalwidth;    // �ܿ��
    public float totalheight;   // �ܸ߶�
    public GameObject[] objectsPrefabs; // �ϰ���Ԥ��������
    public int mapNums;         // ��ͼ����
    public int min = 2;         // ��С��������
    public int max = 4;         // �����������
    public float currenttime = 0;
    private List<GameObject> activeObjects = new List<GameObject>(); // ��ǰ����ϰ����б�
    //private bool isPaused = false; // �Ƿ���ͣ���ɺ�����

    void Start()
    {
        mapNums = 3;
        mainCam = GameObject.FindWithTag("MainCamera");
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        mapHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        totalwidth = mapNums * mapWidth;
        totalheight = mapNums * mapHeight;
        SpawnObjects();
    }

    void Update()
    {
        currenttime += Time.deltaTime;
        if (currenttime > 3f)
        {
            currenttime = 0;
            min++;
            max += 2;

            // �����Ұ��Χ���Ƿ����ϰ���
            if (!CheckIfInCamera())
            {
                DestroyAllObjects(); // �������е�ǰ����ϰ���
                SpawnObjects();  // ������һ���ϰ���
            }
        }
    }

   void SpawnObjects()
    {
        int obstacleCount = Random.Range(min, max);
        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject obstacleToSpawn = objectsPrefabs[Random.Range(0, objectsPrefabs.Length)];
            Vector2 spawnPosition = new Vector2(
                Random.Range(-mapWidth / 2, mapWidth / 2), // �ڵ�ͼ�ֲ���Χ������
                Random.Range(-mapHeight / 2, mapHeight / 2)
            );
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.TransformPoint(spawnPosition), Quaternion.identity);
            obstacle.transform.parent = transform; // ���ϰ�������Ϊ��ͼ���Ӷ���
            activeObjects.Add(obstacle); // �����ɵ��ϰ�����ӵ���б�
        }
       
    }

    bool CheckIfInCamera()
    {
        Vector3 cameraPosition = mainCam.transform.position;
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect; // �������Ұ�İ��
        float cameraHalfHeight = Camera.main.orthographicSize; // �������Ұ�İ��

        foreach (GameObject obstacle in activeObjects)
        {
            if (obstacle == null)
            {
                continue;
            }

            Vector3 obstaclePosition = obstacle.transform.position;

            // �ж��ϰ����Ƿ����������Ұ��
            if (obstaclePosition.x >= cameraPosition.x - cameraHalfWidth &&
                obstaclePosition.x <= cameraPosition.x + cameraHalfWidth &&
                obstaclePosition.y >= cameraPosition.y - cameraHalfHeight &&
                obstaclePosition.y <= cameraPosition.y + cameraHalfHeight)
            {
                // �����Ұ��Χ�����ϰ������true
                return true;
            }
        }

        // �����Ұ��Χ��û���ϰ������false
        return false;
    }

    void DestroyAllObjects()
    {
        foreach (GameObject obstacle in activeObjects)
        {
            if (obstacle != null)
            {
                Destroy(obstacle);
            }
        }
        activeObjects.Clear(); // ��ջ�ϰ����б�
    }
}