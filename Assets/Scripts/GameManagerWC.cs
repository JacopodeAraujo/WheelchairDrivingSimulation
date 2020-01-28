using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerWC : MonoBehaviour {
    StdCtrlInterface std_movement;
    TrailTrackingLog ttlog;
    Communicationwithpython commscript;

    bool isGameOver = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;
    public GameObject player;
    public Camera OVcam;

    private void Start() {
        std_movement = player.GetComponent<StdCtrlInterface>();
        ttlog = player.GetComponentInChildren<TrailTrackingLog>();
        commscript = GetComponent<Communicationwithpython>();
    }

    public void NextLevel() {
        // Next level is loaded in levelComplete.cs
        completeLevelUI.SetActive(true);
    }

    public void EndGame() {
        if (!isGameOver) {
            isGameOver = true;
            std_movement.enabled = false;                           // Disable player movement when destination is reached
            Debug.Log("End game");
            Invoke("RestartLevel", restartDelay);
        }
    }

    void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu() {
        SceneManager.LoadScene("StartMenu");
    }

    private void Update() {
        if (Input.GetKeyDown("r")) {
            EndGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GoToMenu();
        }
        if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.Y))
            commscript.SendStartRecording();
        if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.N))
            commscript.SendEndRecording();
        if (Input.GetKey("p")) {

            if (Input.GetMouseButtonDown(0)) {

                Vector3 mousepos = Input.mousePosition;
                Ray ray = OVcam.ScreenPointToRay(mousepos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.gameObject.layer == 13) {
                        Vector3 spawnPosition = hit.point;
                        spawnPosition.y = 2;
                        player.transform.position = spawnPosition;
                        player.transform.rotation = new Quaternion(0f, 90f, 0f, 1f);
                        //ttlog.SpawnIncrement();

                    }

                }
            }
            if (Input.GetMouseButtonDown(1))
                ttlog.SpawnIncrement();

        }

    }

}
