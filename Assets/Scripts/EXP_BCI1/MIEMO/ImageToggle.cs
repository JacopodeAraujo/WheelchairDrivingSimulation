using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour
{
    public Sprite im_right1;
    public Sprite im_right2;
    public Sprite im_left1;
    public Sprite im_left2;
    public Sprite im_both1;
    public Sprite im_both2;
    public Sprite im_legs1;
    public Sprite im_legs2;


    //public void SetImage(int movement) {
    //    image_code = movement;
    //    switch (movement) {
    //        case 1: GetComponent<Image>().sprite = im_right1; break;
    //        case 2: GetComponent<Image>().sprite = im_left1;  break;
    //        case 3: GetComponent<Image>().sprite = im_both1;  break;
    //        case 4: GetComponent<Image>().sprite = im_legs1;  break;
    //        default: Debug.Log("Switch case error Image Toggle"); GetComponent<Image>().sprite = im_right1; break;
    //    }
    //}
    public void SetImage(GameObject im, int movement) {
        //image_code = movement;
        switch (movement) {
            case 1: im.GetComponent<Image>().sprite = im_right1; break;
            case 2: im.GetComponent<Image>().sprite = im_left1; break;
            case 3: im.GetComponent<Image>().sprite = im_both1; break;
            case 4: im.GetComponent<Image>().sprite = im_legs1; break;
            default: Debug.Log("Switch case error Image Toggle"); im.GetComponent<Image>().sprite = im_right1; break;
        }
    }

    public void ToggleImage(GameObject im) {
        if (im.GetComponent<Image>().sprite == im_right1 || im.GetComponent<Image>().sprite == im_right2) {
            if (im.GetComponent<Image>().sprite == im_right1)
                im.GetComponent<Image>().sprite = im_right2;
            else im.GetComponent<Image>().sprite = im_right1;
        }

        if (im.GetComponent<Image>().sprite == im_left1 || im.GetComponent<Image>().sprite == im_left2) {

            if (im.GetComponent<Image>().sprite == im_left1)
                im.GetComponent<Image>().sprite = im_left2;
            else im.GetComponent<Image>().sprite = im_left1;
        }

        if (im.GetComponent<Image>().sprite == im_both1 || im.GetComponent<Image>().sprite == im_both2) {
            if (im.GetComponent<Image>().sprite == im_both1)
                im.GetComponent<Image>().sprite = im_both2;
            else im.GetComponent<Image>().sprite = im_both1;
        }

        if (im.GetComponent<Image>().sprite == im_legs1 || im.GetComponent<Image>().sprite == im_legs2) {
            if (im.GetComponent<Image>().sprite == im_legs1)
                im.GetComponent<Image>().sprite = im_legs2;
            else im.GetComponent<Image>().sprite = im_legs1;
        }

    }

    public void SetImageText(GameObject imtext, int mi) {
        switch (mi) {
            case 1: imtext.GetComponent<Text>().text = "Right Arm"; break;
            case 2: imtext.GetComponent<Text>().text = "Left Arm"; break;
            //case 3: imtext.GetComponent<Text>().text = "Both Arms"; break;
            case 3: imtext.GetComponent<Text>().text = "Rest"; break;
            case 4: imtext.GetComponent<Text>().text = "Both Legs"; break;
            default: imtext.GetComponent<Text>().text = "FAIL"; break;
        }
    }
}
