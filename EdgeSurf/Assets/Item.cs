using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject mainCam;   // 主摄像机
    public float mapWidth;      // 单个地图的宽度
    public float mapHeight;     // 单个地图的高度
    public float totalwidth;    // 总宽度
    public float totalheight;   // 总高度
    public GameObject[]ItemPrefabs; // 障碍物预制体数组
    public int mapNums;         // 地图数量
    public int min = 2;         // 最小生成数量
    public int max = 4;         // 最大生成数量
    public float currenttime = 0;
    private List<GameObject> activeObjects = new List<GameObject>(); // 当前活动的障碍物列表
    private bool isSpawn = false; // 是否暂停生成和销毁
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
                Random.Range(-mapWidth / 2, mapWidth / 2), // 在地图局部范围内生成
                Random.Range(-mapHeight / 2, mapHeight / 2)
            );
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.TransformPoint(spawnPosition), Quaternion.identity);
            obstacle.transform.parent = transform; // 将障碍物设置为地图的子对象
            activeObjects.Add(obstacle); // 将生成的障碍物添加到活动列表
        }

    }
}
