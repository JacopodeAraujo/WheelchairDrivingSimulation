using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    public Transform tf;
    public Rigidbody rb;
    public Vector3 rotateAngle;
    public Vector3 pushForce;
    

    public bool d_pres = false;
    public bool a_pres = false;
    public bool w_pres = false;
    public bool s_pres = false;

    // Update is called once per frame
    void Update()
    {
        if (d_pres)
        {
            tf.Rotate(rotateAngle);
        }
        if (a_pres)
        {
            tf.Rotate(-rotateAngle);
        }
        if (w_pres)
        {
            if (Math.Abs(tf.position.y-1) < 0.2)
            {
                rb.AddRelativeForce(pushForce);
            }
        }
        if (s_pres)
        {
            if (Math.Abs(tf.position.y - 1) < 0.2)  
            {
                rb.AddRelativeForce(-pushForce);
            }
        }
    }
    void FixedUpdate()
    {
        if (Input.GetKey("d"))
        {
            d_pres = true;
        }
        else d_pres = false;

        if (Input.GetKey("a"))
        {
            a_pres = true;
        }
        else a_pres = false;

        if (Input.GetKey("w"))
        {
            w_pres = true;
        }
        else w_pres = false;

        if (Input.GetKey("s"))
        {
            s_pres = true;
        }
        else s_pres = false;

        if (rb.position.y < -2f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
        if (Input.GetKey("r")) 
        {
            FindObjectOfType<GameManager>().EndGame();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            FindObjectOfType<GameManager>().GoToMenu();
        }
    }
}
