using UnityEngine;


public class Battatmenu : MonoBehaviour
{

    public GameObject menupanel;
    public GameObject settingpanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tatmenu()
    {
        menupanel.SetActive(true);

    }

   public void Tatsetting()
    {
        settingpanel.SetActive(false);
    }
}
