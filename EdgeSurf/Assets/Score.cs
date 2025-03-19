using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public PlayerController player;
    public float MaxScore;

    private float ScoreBefore; // 记录上一帧的分数  
    public float speed = 5f; // 过渡速度  
    private Image image;
    public Text text1; 
    public Text text2;
    public Color defaultColor = Color.green; // 默认血条颜色
    public Color newRecordColor = Color.yellow; // 新高分时的血条颜色

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.color = defaultColor; // 初始化血条颜色

        if (player != null)
        {
            ScoreBefore = player.GetScore();
        }
        MaxScore = PlayerPrefs.GetFloat("newMax", 0f); // 最高记录分
    }

    void Update()
    {
        if (player == null) return;

        float currentScore = player.GetScore();

        // 如果当前分数超过最高记录分
        if (currentScore > MaxScore)
        {
                image.color = newRecordColor; // 设置血条颜色为黄色
                text2.text="新高分！"; // 输出新高分提示
        }
        else
        {
            text2.text = "";
        }
        // 如果游戏未暂停，进行平滑过渡  
        if (Time.timeScale > 0)
        {
            ScoreBefore = Mathf.Lerp(ScoreBefore, currentScore, Time.deltaTime * speed);
        }
        else
        {
            // 在暂停状态下直接更新 ScoreBefore  
            ScoreBefore = currentScore;
        }

        // 更新血条填充比例
        image.fillAmount = ScoreBefore / MaxScore;

        // 更新分数文本  
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (text1!= null)
        {
            text1.text = Mathf.RoundToInt(ScoreBefore).ToString() + " / " + Mathf.RoundToInt(MaxScore).ToString();
        }
    }
}