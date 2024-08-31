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
    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }
        public void OnButtonPress()
    {
        Btn_Image.sprite = Pressed_Sprite; // ��ư�� ������ �� ��������Ʈ ����
    }

    public void OnButtonRelease(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // �� ��ȯ
    }
}