using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hp : MonoBehaviour
{
    public Text text1;
   public  Text text2;
    public PlayerController playerController;
    public float hp;
    public float maxhp;
    public int boost;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    { hp=playerController.GetHp();
        maxhp=playerController.GetMaxHp();
        boost=playerController.Getboost();
        text1.text = Mathf.RoundToInt(hp).ToString() + " / " + Mathf.RoundToInt(maxhp).ToString();
        text2.text = Mathf.RoundToInt(boost).ToString()+"/ 3";
    }
}
