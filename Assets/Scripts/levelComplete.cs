
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelComplete : MonoBehaviour
{
    public static int repeat = 2;
    public void LoadNextLevel()
    {
        string scene = SceneManager.GetActiveScene().name;
        //Debug.Log("TEST" + repeat);
        if (scene.Equals("Level2WC")) {
            if (repeat > 0) {
                repeat--;
                SceneManager.LoadScene(scene);
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                repeat = 2;
            }
        }
        else if (scene.Equals("Level3WC")) {
            if (repeat > 0) {
                repeat--;
                SceneManager.LoadScene(scene);
            }
            else {
                SceneManager.LoadScene("Level4WC" + DetailsScript.GetLcomb1());
                repeat = 2;
            }

        }
        else if (scene.Contains("Level4WC")) {
            SceneManager.LoadScene("ImagePause1");
        }
        else if (scene.Contains("ImagePause1")) {
            SceneManager.LoadScene("Level5WC" + DetailsScript.GetLcomb2());
        }
        else if (scene.Contains("Level5WC")) {
            SceneManager.LoadScene("Level6WC" + DetailsScript.GetLcomb3());
        }
        else if (scene.Contains("Level6WC")) {
            SceneManager.LoadScene("ImagePause2");
        }
        else if (scene.Contains("ImagePause2")) {
            Debug.Log("Complete");
            LoadMenu();
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
