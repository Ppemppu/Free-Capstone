using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public Sprite Pressed_Sprite;

    private Image Btn_Image;

    private bool isPressed;


    private void Start()
    {
        isPressed = false;
        Btn_Image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            Btn_Image.sprite = Pressed_Sprite;
        }
        else if(Input.GetMouseButtonUp(0) && isPressed)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("In Game");
    }

}