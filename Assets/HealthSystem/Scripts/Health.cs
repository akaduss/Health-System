using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    #region test
    public float damageAmount;
    public float healAmount;
    #endregion

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    public delegate void HealthChangedDelegate(float newHealth, float maxHealth);
    public event HealthChangedDelegate OnHealthChanged;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer visualRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private float feedbackDuration = 0.2f;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 1f;
    private bool isInvulnerable = false;


    private void Start()
    {
        InitializeHealth();
        InitializeVisualFeedback();
        RegisterWithHealthManager();
    }

    private void Awake()
    {
        InitializeHealth();
        InitializeVisualFeedback();
        RegisterWithHealthManager();
    }

    private void OnDestroy()
    {
        UnregisterFromHealthManager();
    }

    private void RegisterWithHealthManager()
    {
        HealthManager.Instance.RegisterHealth(this);
    }

    private void UnregisterFromHealthManager()
    {
        HealthManager.Instance.UnregisterHealth(this);
    }

    private void InitializeHealth()
    {
        currentHealth = maxHealth;
    }

    private void InitializeVisualFeedback()
    {
        if (visualRenderer == null)
        {
            Debug.LogWarning("Renderer component not found. Visual feedback will not work.");
        }
    }

    private void ShowFeedback(Color color)
    {
        if (visualRenderer != null)
        {
            visualRenderer.material.color = color;
            Invoke(nameof(ResetFeedback), feedbackDuration);
        }
    }

    private void ResetFeedback()
    {
        if (visualRenderer != null)
        {
            visualRenderer.material.color = Color.white;
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
            ApplyDamage(damage);
            ShowFeedback(damageColor);
            SetInvulnerability();
            CheckDeath();
        }
    }

    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void SetInvulnerability()
    {
        isInvulnerable = true;
        Invoke(nameof(ResetInvulnerability), invulnerabilityDuration);
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    public void Heal(float amount)
    {
        ApplyHealing(amount);
        ShowFeedback(healColor);
    }

    private void ApplyHealing(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void HandleDeath()
    {
        if (TryGetComponent<IDeathHandler>(out var deathHandler))
        {
            deathHandler.HandleDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}