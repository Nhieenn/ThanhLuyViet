using UnityEngine;
using UnityEngine.UI;

public class Hpbar_Enemy : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    [Header("-------------Hp bar")]
    [SerializeField] private Slider Hp_Slider;
    [SerializeField] private Image Hp_color;

    void Start()
    {
        enemy = FindAnyObjectByType<Enemy>();
    }

    void Update()
    {
        Hp_Slider.value = enemy.CurHp;
        HPCOLOR();
    }

    public void HPCOLOR()
    {
        float hpPercent = (float)enemy.CurHp / enemy.MaxHp;

        if (hpPercent >= 0.75f)
        {
            Hp_color.color = Color.green;
        }
        else if (hpPercent >= 0.5f)
        {
            Hp_color.color = Color.yellow;
        }
        else if (hpPercent >= 0.25f)
        {
            Hp_color.color = new Color(1f, 0.5f, 0f); // cam
        }
        else
        {
            Hp_color.color = Color.red;
        }
    }
}
