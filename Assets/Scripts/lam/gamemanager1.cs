using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamemanager1 : MonoBehaviour
{
    [SerializeField]
    private Text welcomeText;

    void Start()
    {
        // Chỉ hiển thị message nếu có Text component
        if (welcomeText != null)
        {
            ShowWelcomeMessage();
        }
        else
        {
            // Tìm Text component trong scene
            welcomeText = FindObjectOfType<Text>();
            if (welcomeText != null)
            {
                ShowWelcomeMessage();
            }
            else
            {
                // Nếu không có Text, chỉ log message
                Debug.Log($"Welcome {References.userName} to our game");
            }
        }
    }

    private void ShowWelcomeMessage()
    {
        if (welcomeText != null)
        {
            welcomeText.text = $"Welcome {References.userName} to our game";
        }
    }

    public void gotodangnhap()
    {
        SceneManager.LoadScene("FirebaseLogin");
    }
}
