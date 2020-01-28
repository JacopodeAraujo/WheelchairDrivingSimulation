using UnityEngine;

public class DestinationReach : MonoBehaviour
{
    public Material destMatTransparent;
    public Material destMatNotTransparent;
    public Rigidbody PlayerRB;
    public GameManager gameManager;


    public void matTransparent(bool isTransparent)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (isTransparent)
            {
                rend.material = destMatTransparent;
            }
            else rend.material = destMatNotTransparent;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        matTransparent(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        { 
            if (PlayerRB.velocity == new Vector3(0, 0, 0))
            {
                Debug.Log("You won");
                gameManager.NextLevel();

            }
            else { Debug.Log("Stay put"); }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        matTransparent(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
