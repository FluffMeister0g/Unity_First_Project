using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{

    public static int Score;
    public static int HighScore;

    public TMP_Text timerText;
    public float secondsCount;
    public int minuteCount;

    public bool isPaused = false;
    public GameObject pauseMenu;
    public PlayerControler playerData;
    public Image healthBar;
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI clipCounter;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("Player").GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerData.health <= 0)
        {
            LoadLevel(0);
        }

        healthBar.fillAmount = Mathf.Clamp(((float)playerData.health / (float)playerData.maxHealth), 0, 1);
        ammoCounter.text = "Clip" + playerData.currentClip + "/" + playerData.maxAmmo;
        ammoCounter.text = "Ammo: " + playerData.currentAmmo;

        if (!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }

        else if (isPaused && Input.GetKeyDown(KeyCode.Escape))
           Resume();
        //HighScore


        //timer
        UpdateTimerUI();
    }
    public void UpdateTimerUI()
    {
        secondsCount += Time.deltaTime;
        timerText.text = minuteCount + "m:" + (int)secondsCount + "s";
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        
    }
    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
        Time.timeScale = 1;
    }
}
