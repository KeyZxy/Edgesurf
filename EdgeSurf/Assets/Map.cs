using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject mainCam;   // 主摄像机
    public float mapWidth;      // 单个地图的宽度
    public float mapHeight;     // 单个地图的高度
    public int mapNums;         // 地图数量
    public float totalwidth;    // 总宽度
    public float totalheight;   // 总高度

    void Start()
    {
        mapNums = 3;
        mainCam = GameObject.FindWithTag("MainCamera");
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        mapHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        totalwidth = mapNums * mapWidth;
        totalheight = mapNums * mapHeight;
    }

    void Update()
    {
        Vector3 temp = transform.position;

        // X 轴移动逻辑
        if (mainCam.transform.position.x > transform.position.x + totalwidth / 2)
        {
            temp.x += totalwidth;
            transform.position = temp;
        }
        else if (mainCam.transform.position.x < transform.position.x - totalwidth / 2)
        {
            temp.x -= totalwidth;
            transform.position = temp;
        }

        // Y 轴移动逻辑
        if (mainCam.transform.position.y > transform.position.y + totalheight / 2)
        {
            temp.y += totalheight;
            transform.position = temp;
        }
        else if (mainCam.transform.position.y < transform.position.y - totalheight / 2)
        {
            temp.y -= totalheight;
            transform.position = temp;
        }
    }
}