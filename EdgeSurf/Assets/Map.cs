using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject mainCam;   // �������
    public float mapWidth;      // ������ͼ�Ŀ��
    public float mapHeight;     // ������ͼ�ĸ߶�
    public int mapNums;         // ��ͼ����
    public float totalwidth;    // �ܿ��
    public float totalheight;   // �ܸ߶�

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

        // X ���ƶ��߼�
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

        // Y ���ƶ��߼�
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