using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image foregroundBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Optional Settings")]
    [SerializeField] private bool useCustomColors = false;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color healColor = Color.green;

    private float maxWidth;

    private Health healthComponent;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Assuming the foregroundBar is a child of the health bar and fills it horizontally
        maxWidth = foregroundBar.rectTransform.sizeDelta.x;

        // Assuming there is a HealthManager in the scene
        HealthManager healthManager = HealthManager.Instance;

        if (healthManager != null)
        {
            // Assuming there is only one player with Health component
            healthComponent = healthManager.PlayerHealth;

            if (healthComponent != null)
            {
                healthComponent.OnHealthChanged += UpdateUI;
                UpdateUI(healthComponent.CurrentHealth, healthComponent.MaxHealth);
            }
            else
            {
                Debug.LogError("Player Health component not found in the HealthManager.");
            }
        }
        else
        {
            Debug.LogError("HealthManager not found in the scene.");
        }
    }

    private void UpdateUI(float currentHealth, float maxHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);
        float newWidth = maxWidth * healthPercentage;

        // Update the foreground bar size
        foregroundBar.rectTransform.sizeDelta = new Vector2(newWidth, foregroundBar.rectTransform.sizeDelta.y);

        // Update the health text with integer values
        if (healthText != null)
        {
            int roundedCurrentHealth = Mathf.RoundToInt(currentHealth);
            int roundedMaxHealth = Mathf.RoundToInt(maxHealth);
            healthText.text = $"{roundedCurrentHealth} / {roundedMaxHealth}";
        }

        // Update the bar color based on damage or healing
        UpdateBarColor();
    }

    private void UpdateBarColor()
    {
        if (useCustomColors)
        {
            Color barColor = healthComponent.CurrentHealth < healthComponent.MaxHealth ? damageColor : healColor;
            foregroundBar.color = barColor;
        }
    }
}
