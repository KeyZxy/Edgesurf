using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI; // ��ͣ�˵���UI���  
    public GameObject mainMenuUI;
    public static bool GameIsPaused = false;
    public PlayerController playerController;
    void Start()
    {
      pauseMenuUI.SetActive(false);
      mainMenuUI.SetActive(false);
    }

    void Update()
    {
       
        // ����ESC��ʱ������ͣ  
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Space)) {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    // �ָ���Ϸ  
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // ������ͣ�˵�  
        Time.timeScale = 1f;          // �ָ���Ϸʱ������  
        GameIsPaused = false;         // ������ͣ״̬  
        playerController.isInputEnabled = true;

    }
    // �������˵������浱ǰ����
    public void SaveAndReturnToMainMenu()
    {
        Time.timeScale = 0f;          // ��ͣ��Ϸʱ������  
        SaveGameProgress();
        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false );
        
    }
    void SaveGameProgress()
    {
        float sc = playerController.GetScore();
        float maxsc = playerController.GetmaxDistance();
        Debug.Log("Current Score: " + sc);
        Debug.Log("Current Max Score: " + maxsc);

        // �����ǰ����������߷֣�������߷�
        if (sc > maxsc || maxsc == 0)
        {
            PlayerPrefs.SetFloat("newMax", sc);
            PlayerPrefs.Save();  // ����
            playerController.SetmaxDistance(sc); // ͬ�����½ű��е�MaxDistance
            Debug.Log("New Max Score Saved: " + sc);
        }
    }

    
    // ����ͳ����Ϣ
    public void ResetStatistics()
    {
        playerController.SetmaxDistance(0);
        PlayerPrefs.DeleteKey("newMax");  // ɾ���������߷�
        PlayerPrefs.Save();
    }
    // ��ͣ��Ϸ  
    void Paused()
    {

        pauseMenuUI.SetActive(true);  // ��ʾ��ͣ�˵�  
        Time.timeScale = 0f;          // ��ͣ��Ϸʱ������  
        GameIsPaused = true;          // ������ͣ״̬  
       playerController.isInputEnabled = false;
    }

    // �˳���Ϸ  
    public void QuitGame()
    {
        Debug.Log("Quitting game...");


#if UNITY_EDITOR
        // ����ڱ༭���У�ֹͣ����ģʽ  
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����ڹ�������Ϸ�У��˳�Ӧ�ó���  
        Application.Quit();  
#endif
    }
}