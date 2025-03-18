using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borad : MonoBehaviour
{
    public float currenttime = 0;
    public GameObject board;
    // Start is called before the first frame update
    void Start()
    {
     board.SetActive(false);  
    }

    // Update is called once per frame
    void Update()
    {
        currenttime += Time.deltaTime;
        if (currenttime > 5f)
        {
            
           board.SetActive(true);
        }
    }
}
