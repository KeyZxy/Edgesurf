using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject mainCam;   // 主摄像机
    public float mapWidth;      // 单个地图的宽度
    public float mapHeight;     // 单个地图的高度
    public float totalwidth;    // 总宽度
    public float totalheight;   // 总高度
    public GameObject[] objectsPrefabs; // 障碍物预制体数组
    public int mapNums;         // 地图数量
    public int min = 2;         // 最小生成数量
    public int max = 4;         // 最大生成数量
    public float currenttime = 0;
    private List<GameObject> activeObjects = new List<GameObject>(); // 当前活动的障碍物列表
    //private bool isPaused = false; // 是否暂停生成和销毁

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

            // 检查视野范围内是否有障碍物
            if (!CheckIfInCamera())
            {
                DestroyAllObjects(); // 销毁所有当前活动的障碍物
                SpawnObjects();  // 生成新一波障碍物
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
                Random.Range(-mapWidth / 2, mapWidth / 2), // 在地图局部范围内生成
                Random.Range(-mapHeight / 2, mapHeight / 2)
            );
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.TransformPoint(spawnPosition), Quaternion.identity);
            obstacle.transform.parent = transform; // 将障碍物设置为地图的子对象
            activeObjects.Add(obstacle); // 将生成的障碍物添加到活动列表
        }
       
    }

    bool CheckIfInCamera()
    {
        Vector3 cameraPosition = mainCam.transform.position;
        float cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect; // 摄像机视野的半宽
        float cameraHalfHeight = Camera.main.orthographicSize; // 摄像机视野的半高

        foreach (GameObject obstacle in activeObjects)
        {
            if (obstacle == null)
            {
                continue;
            }

            Vector3 obstaclePosition = obstacle.transform.position;

            // 判断障碍物是否在摄像机视野内
            if (obstaclePosition.x >= cameraPosition.x - cameraHalfWidth &&
                obstaclePosition.x <= cameraPosition.x + cameraHalfWidth &&
                obstaclePosition.y >= cameraPosition.y - cameraHalfHeight &&
                obstaclePosition.y <= cameraPosition.y + cameraHalfHeight)
            {
                // 如果视野范围内有障碍物，返回true
                return true;
            }
        }

        // 如果视野范围内没有障碍物，返回false
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
        activeObjects.Clear(); // 清空活动障碍物列表
    }
}