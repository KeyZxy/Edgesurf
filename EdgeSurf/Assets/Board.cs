using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borad : MonoBehaviour
{
    public float currenttime = 0;
    public GameObject board;
    public GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
     board.SetActive(false);  
        Enemy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currenttime += Time.deltaTime;
        if (currenttime > 1f)
        {
            
           board.SetActive(true);
        }
        if (currenttime > 5f)
        {
            Enemy.SetActive(true);

        }
    }
}
