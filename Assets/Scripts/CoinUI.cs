using UnityEngine;
using UnityEngine.UI;
using TMPro; // Thêm namespace cho TextMeshPro

public class CoinUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinText; // Thay đổi từ Text sang TextMeshProUGUI
    [SerializeField] private GameObject coinIcon;
    [SerializeField] private Animator coinAnimator;
    
    [Header("Animation")]
    [SerializeField] private string addCoinTrigger = "AddCoin";
    [SerializeField] private string spendCoinTrigger = "SpendCoin";
    
    private void Start()
    {
        // Tìm CoinManager và đăng ký UI
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetCoinUI(coinText, coinIcon);
        }
        else
        {
            Debug.LogWarning("CoinManager not found! Make sure CoinManager is in the scene.");
        }
        
        // Tìm UI elements nếu không được gán
        if (coinText == null)
        {
            coinText = GetComponentInChildren<TextMeshProUGUI>(); // Tìm TextMeshProUGUI thay vì Text
        }
        
        if (coinIcon == null)
        {
            coinIcon = transform.Find("CoinIcon")?.gameObject;
        }
        
        if (coinAnimator == null)
        {
            coinAnimator = GetComponent<Animator>();
        }
    }
    
    private void Update()
    {
        // Cập nhật UI nếu CoinManager có sẵn
        if (CoinManager.Instance != null && coinText != null)
        {
            coinText.text = CoinManager.Instance.CurrentCoins.ToString();
        }
    }
    
    // Gọi từ bên ngoài để trigger animation
    public void TriggerAddCoinAnimation()
    {
        if (coinAnimator != null)
        {
            coinAnimator.SetTrigger(addCoinTrigger);
        }
    }
    
    public void TriggerSpendCoinAnimation()
    {
        if (coinAnimator != null)
        {
            coinAnimator.SetTrigger(spendCoinTrigger);
        }
    }
    
    // Gọi từ CoinManager để trigger animation
    public void OnCoinsAdded(int amount)
    {
        TriggerAddCoinAnimation();
    }
    
    public void OnCoinsSpent(int amount)
    {
        TriggerSpendCoinAnimation();
    }
}
