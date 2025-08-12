using UnityEngine;

public class EndPosDamage : MonoBehaviour
{
    [Header("References")]
    public SimpleHealthText healthSystem;
    
    [Header("Damage Settings")]
    public bool showDamagePopup = true;
    public GameObject damagePopupPrefab;
    public float popupDuration = 2f;
    
    [Header("Audio")]
    public AudioClip damageSound;
    public AudioClip criticalDamageSound;
    
    [Header("Effects")]
    public GameObject damageEffect;
    public bool shakeCamera = true;
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.3f;
    
    private Camera mainCamera;
    private Vector3 originalCameraPosition;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
        }
        
        // Auto-find health system if not assigned
        if (healthSystem == null)
        {
            healthSystem = FindObjectOfType<SimpleHealthText>();
            if (healthSystem == null)
            {
                Debug.LogWarning("EndPosDamage: No SimpleHealthText found in scene!");
            }
        }

        if (healthSystem == null)
        {
            Debug.LogWarning("EndPosDamage: No SimpleHealthText found under Canvas/Info/Heart/Text!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.enemyData != null)
            {
                int damage = enemy.enemyData.attackDamage;
                
                // Check if this will kill the player
                bool willKillPlayer = healthSystem != null && healthSystem.currentHealth <= damage;
                
                // Apply damage
                if (healthSystem != null)
                {
                    healthSystem.TakeDamage(damage);
                    Debug.Log($"💥 Enemy đến EndPos! Trừ {damage} máu. Health còn lại: {healthSystem.currentHealth}");
                }
                else
                {
                    Debug.LogError("EndPosDamage: Health system is null!");
                }

                // Show damage popup
                if (showDamagePopup)
                {
                    ShowDamagePopup(damage, other.transform.position);
                }
                
                // Play sound
                PlayDamageSound(willKillPlayer);
                
                // Show effects
                ShowDamageEffects(other.transform.position);
                
                // Camera shake
                if (shakeCamera && mainCamera != null)
                {
                    StartCoroutine(ShakeCamera());
                }
                
                // Destroy enemy
                Destroy(other.gameObject);
            }
            else
            {
                Debug.LogWarning("Enemy không có EnemyData component!");
            }
        }
    }
    
    void ShowDamagePopup(int damage, Vector3 position)
    {
        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
            TextMesh damageText = popup.GetComponentInChildren<TextMesh>();
            if (damageText != null)
            {
                damageText.text = $"-{damage}";
                damageText.color = Color.red;
            }
            
            // Destroy popup after duration
            Destroy(popup, popupDuration);
        }
        else
        {
            // Create simple text popup
            GameObject popup = new GameObject("Damage Popup");
            popup.transform.position = position;
            
            TextMesh text = popup.AddComponent<TextMesh>();
            text.text = $"-{damage}";
            text.color = Color.red;
            text.fontSize = 20;
            
            // Animate popup
            StartCoroutine(AnimateDamagePopup(popup));
        }
    }
    
    System.Collections.IEnumerator AnimateDamagePopup(GameObject popup)
    {
        Vector3 startPos = popup.transform.position;
        Vector3 endPos = startPos + Vector3.up * 2f;
        float elapsed = 0f;
        
        while (elapsed < popupDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / popupDuration;
            
            // Move up
            popup.transform.position = Vector3.Lerp(startPos, endPos, progress);
            
            // Fade out
            TextMesh text = popup.GetComponent<TextMesh>();
            if (text != null)
            {
                Color color = text.color;
                color.a = 1f - progress;
                text.color = color;
            }
            
            yield return null;
        }
        
        Destroy(popup);
    }
    
    void PlayDamageSound(bool isCritical)
    {
        AudioClip soundToPlay = isCritical ? criticalDamageSound : damageSound;
        
        if (soundToPlay != null)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
        }
    }
    
    void ShowDamageEffects(Vector3 position)
    {
        if (damageEffect != null)
        {
            Instantiate(damageEffect, position, Quaternion.identity);
        }
    }
    
    System.Collections.IEnumerator ShakeCamera()
    {
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / shakeDuration;
            float intensity = shakeIntensity * (1f - progress);
            
            Vector3 randomOffset = Random.insideUnitSphere * intensity;
            mainCamera.transform.position = originalCameraPosition + randomOffset;
            
            yield return null;
        }
        
        // Reset camera position
        mainCamera.transform.position = originalCameraPosition;
    }
    
    [ContextMenu("Test Damage 10")]
    public void TestDamage10()
    {
        if (healthSystem != null)
        {
            healthSystem.TakeDamage(10);
        }
    }
    
    [ContextMenu("Test Damage 50")]
    public void TestDamage50()
    {
        if (healthSystem != null)
        {
            healthSystem.TakeDamage(50);
        }
    }
    
    [ContextMenu("Test Damage 100")]
    public void TestDamage100()
    {
        if (healthSystem != null)
        {
            healthSystem.TakeDamage(100);
        }
    }
    
    [ContextMenu("Test Camera Shake")]
    public void TestCameraShake()
    {
        if (shakeCamera && mainCamera != null)
        {
            StartCoroutine(ShakeCamera());
        }
    }
}