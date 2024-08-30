using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public Sprite Pressed_Sprite;
    public Image Btn_Image;
    public void Start()
    {
        Btn_Image = GetComponent<Image>();
    }

    public void OnButtonPress()
    {
        Btn_Image.sprite = Pressed_Sprite; // 버튼이 눌렸을 때 스프라이트 변경
    }

    public void OnButtonRelease(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 전환
    }

}