using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI; // 暂停菜单的UI面板  
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
       
        // 按下ESC键时触发暂停  
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

    // 恢复游戏  
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // 隐藏暂停菜单  
        Time.timeScale = 1f;          // 恢复游戏时间流动  
        GameIsPaused = false;         // 更新暂停状态  
        playerController.isInputEnabled = true;

    }
    // 返回主菜单并保存当前进度
    public void SaveAndReturnToMainMenu()
    {
        Time.timeScale = 0f;          // 暂停游戏时间流动  
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

        // 如果当前分数大于最高分，更新最高分
        if (sc > maxsc || maxsc == 0)
        {
            PlayerPrefs.SetFloat("newMax", sc);
            PlayerPrefs.Save();  // 保存
            playerController.SetmaxDistance(sc); // 同步更新脚本中的MaxDistance
            Debug.Log("New Max Score Saved: " + sc);
        }
    }

    
    // 重置统计信息
    public void ResetStatistics()
    {
        playerController.SetmaxDistance(0);
        PlayerPrefs.DeleteKey("newMax");  // 删除保存的最高分
        PlayerPrefs.Save();
    }
    // 暂停游戏  
    void Paused()
    {

        pauseMenuUI.SetActive(true);  // 显示暂停菜单  
        Time.timeScale = 0f;          // 暂停游戏时间流动  
        GameIsPaused = true;          // 更新暂停状态  
       playerController.isInputEnabled = false;
    }

    // 退出游戏  
    public void QuitGame()
    {
        Debug.Log("Quitting game...");


#if UNITY_EDITOR
        // 如果在编辑器中，停止播放模式  
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果在构建的游戏中，退出应用程序  
        Application.Quit();  
#endif
    }
}