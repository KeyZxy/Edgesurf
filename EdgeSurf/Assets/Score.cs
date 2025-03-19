using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public PlayerController player;
    public float MaxScore;

    private float ScoreBefore; // ��¼��һ֡�ķ���  
    public float speed = 5f; // �����ٶ�  
    private Image image;
    public Text text1; 
    public Text text2;
    public Color defaultColor = Color.green; // Ĭ��Ѫ����ɫ
    public Color newRecordColor = Color.yellow; // �¸߷�ʱ��Ѫ����ɫ

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.color = defaultColor; // ��ʼ��Ѫ����ɫ

        if (player != null)
        {
            ScoreBefore = player.GetScore();
        }
        MaxScore = PlayerPrefs.GetFloat("newMax", 0f); // ��߼�¼��
    }

    void Update()
    {
        if (player == null) return;

        float currentScore = player.GetScore();

        // �����ǰ����������߼�¼��
        if (currentScore > MaxScore)
        {
                image.color = newRecordColor; // ����Ѫ����ɫΪ��ɫ
                text2.text="�¸߷֣�"; // ����¸߷���ʾ
        }
        else
        {
            text2.text = "";
        }
        // �����Ϸδ��ͣ������ƽ������  
        if (Time.timeScale > 0)
        {
            ScoreBefore = Mathf.Lerp(ScoreBefore, currentScore, Time.deltaTime * speed);
        }
        else
        {
            // ����ͣ״̬��ֱ�Ӹ��� ScoreBefore  
            ScoreBefore = currentScore;
        }

        // ����Ѫ��������
        image.fillAmount = ScoreBefore / MaxScore;

        // ���·����ı�  
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