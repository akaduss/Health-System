// Health.cs

using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    public float maxHealth = 100f;
    private float currentHealth;

    // Events for external systems to listen to
    public delegate void HealthChangedDelegate(float newHealth, float maxHealth);
    public event HealthChangedDelegate OnHealthChanged;

    private Renderer entityRenderer;
    public Color damageColor = Color.red;
    public Color healColor = Color.green;
    public float feedbackDuration = 0.2f;
    public float invulnerabilityDuration = 1f;
    private bool isInvulnerable = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // Get the renderer component for visual feedback
        entityRenderer = GetComponent<Renderer>();
        if (entityRenderer == null)
        {
            Debug.LogWarning("Renderer component not found. Visual feedback will not work.");
        }
    }

    private void ShowFeedback(Color color)
    {
        if (entityRenderer != null)
        {
            entityRenderer.material.color = color;
            Invoke("ResetFeedback", feedbackDuration);
        }
    }

    private void ResetFeedback()
    {
        if (entityRenderer != null)
        {
            entityRenderer.material.color = Color.white;
        }
    }

    private void ResetInvulnerability()
    {
        isInvulnerable = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            // Trigger the event when health changes
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            // Show visual feedback for damage
            ShowFeedback(damageColor);

            // Set invulnerability
            isInvulnerable = true;
            Invoke("ResetInvulnerability", invulnerabilityDuration);

            if (currentHealth == 0f)
            {
                Die();
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Trigger the event when health changes
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Show visual feedback for healing
        ShowFeedback(healColor);
    }

    private void Die()
    {
        // Implement death logic here
        Debug.Log($"{gameObject.name} has died.");
    }
}
