using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMovement movement;
    bool isGameOver = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    public void NextLevel()
    {
        completeLevelUI.SetActive(true);
    }

    public void EndGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            movement.enabled = false;                           // Disable player movement when destination is reached
            Debug.Log("End game");
            Invoke("RestartLevel", restartDelay);
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
