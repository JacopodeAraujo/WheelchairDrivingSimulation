using UnityEngine;
using UnityEngine.UI;

public class ImageToggleDir : MonoBehaviour
{

    public Sprite im_right;
    public Sprite im_left;

    
    // Sets arrow image to point to right direction if "imageRight"
    public void SetImage(bool imageRight) {
        if (imageRight)
            GetComponent<Image>().sprite = im_right;
        else GetComponent<Image>().sprite = im_left;
    }
}
