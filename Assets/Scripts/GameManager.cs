using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene reloading
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance for easy access

    public GameObject Message;
    public Text MessageText;
    public Text MagicanHealth;
    public int winCount = 1; // Number of monsters to defeat
    public int monstersDefeated = 0; // Track defeated monsters
    public bool isGameOver = false; // Flag to track game state

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Avoid duplicate GameManagers
        }
        DontDestroyOnLoad(gameObject); // Persist GameManager across scenes (if needed)
        Message.SetActive(false);

    }

    public void ChangeHealth(int x, int max)
    {
        MagicanHealth.text = $"{StringConstants.Health} {x}/{max}";
    }

    void WinGame()
    {
        ShowMessage(StringConstants.YouWin);
        isGameOver = true;
        Debug.Log(StringConstants.YouWin);
    }

    public void LoseGame()
    {
        ShowMessage(StringConstants.YouLose);
        isGameOver = true;
        Debug.Log(StringConstants.YouLose);

        //StartCoroutine(nameof(WaitToRestartGame));
    }

    IEnumerator WaitToRestartGame()
    {
        ShowMessage("Enter R to restart the game");
        while (!Input.GetKeyDown(KeyCode.R)) {
            yield return null;
        }
        RestartGame();
    }

    public bool MonsterDefeated() // Function called by monsters on defeat
    {
        monstersDefeated++;
        if (monstersDefeated >= winCount)
        {
            WinGame();
            return true;
        }
        return false;
    }

    void RestartGame()
    {
        Message.SetActive(false);

        monstersDefeated = 0;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        Debug.Log("Restarting Game...");
    }

    void ShowMessage(string text)
    {
        MessageText.text = text;
        Message.SetActive(true);
    }
}
