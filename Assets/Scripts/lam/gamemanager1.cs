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
        ShowWelcomeMessage();
    }

    private void ShowWelcomeMessage()
    {
        welcomeText.text = $"Welcome {References.userName} to our game";
    }

    public void gotodangnhap()
    {
        SceneManager.LoadScene("FirebaseLogin");
    }
}
