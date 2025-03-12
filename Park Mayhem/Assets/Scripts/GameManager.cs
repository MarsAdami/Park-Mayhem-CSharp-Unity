using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public float killCount;
    public Text killCountText;
    public GameObject PauseMenuUI;
    public GameObject winLevelUI;
    public Text highCountText;

    public bool gameIsPaused = false; 

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void WinLevel()
    {
        winLevelUI.SetActive(true);
        highCountText.text = killCount + " Wizard Killed";
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void AppQuit()
    {
        Application.Quit();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
