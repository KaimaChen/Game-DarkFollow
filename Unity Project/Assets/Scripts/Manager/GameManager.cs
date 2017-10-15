using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager {
    public static bool IsPause = false;
    
	public static void GameOver()
    {
        Pause();
        UIManager.Instance.ShowRestartPage();
    }

    public static void Pause()
    {
        IsPause = true;
        Time.timeScale = 0;
    }

    public static void Restart()
    {
        IsPause = false;
        Time.timeScale = 1;
        ShadowManager.Instance.Shadows.Clear();
        SceneManager.LoadScene("Main");
    }

    public static void Quit()
    {
        Application.Quit();
    }
}
