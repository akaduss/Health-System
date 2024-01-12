// HealthUI.cs

using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health healthComponent;
    public Text healthText;

    private void Start()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged += UpdateHealthText;
            UpdateHealthText(healthComponent.currentHealth, healthComponent.maxHealth);
        }
        else
        {
            Debug.LogWarning("Health component not assigned to HealthUI.");
        }
    }

    private void UpdateHealthText(float newHealth, float maxHealth)
    {
        healthText.text = $"Health: {newHealth}/{maxHealth}";
    }

    private void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged -= UpdateHealthText;
        }
    }
}
