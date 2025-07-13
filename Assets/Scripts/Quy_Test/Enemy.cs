using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 200;
    public int CurHp;
    void Start()
    {
        CurHp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurHp <= 0)
        {
            Debug.Log("Died");
            Destroy(gameObject, 3f);
        }
    }
}
