using System;
using System.Collections;
using UnityEngine;

namespace Akadus.HealthSystem
{
    public class Health : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private HealthConfig healthConfig;
        [SerializeField] private Renderer visualRenderer;
        [SerializeField] private bool showVisualFeedback = true;
        [SerializeField] private float currentHealth;

        private bool isInvulnerable = false;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => healthConfig.maxHealth;
        public Color DamageColor => healthConfig.damageColor;
        public Color HealColor => healthConfig.healColor;

        public event Action OnHealthChanged;

        private void Start()
        {
            currentHealth = healthConfig.maxHealth;
            OnHealthChanged?.Invoke();

            if (visualRenderer == null)
            {
                Debug.LogWarning("Renderer component not found. Visual feedback will not work.");
            }

        }

        private void ShowFeedback(Color color, float duration)
        {
            if (visualRenderer != null && showVisualFeedback)
            {
                visualRenderer.material.color = color;
                Invoke(nameof(ResetFeedback), duration);
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
                ShowFeedback(DamageColor, healthConfig.damageFeedbackDuration);
                SetInvulnerability();
                CheckDeath();
            }
        }

        private void ApplyDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, healthConfig.maxHealth);
            OnHealthChanged?.Invoke();
        }

        private void SetInvulnerability()
        {
            isInvulnerable = true;
            Invoke(nameof(ResetInvulnerability), healthConfig.invulnerabilityDuration);
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
            ShowFeedback(HealColor, healthConfig.healFeedbackDuration);
        }

        private void ApplyHealing(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, healthConfig.maxHealth);
            OnHealthChanged?.Invoke();
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

        private IEnumerator DamageOverTime(float damageOverTimeInterval, float damagePerInterval, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                TakeDamage(damagePerInterval);
                yield return new WaitForSeconds(damageOverTimeInterval);
                elapsedTime += damageOverTimeInterval;
            }
        }

        private IEnumerator HealOverTime(float healOverTimeInterval, float healPerInterval, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                Heal(healPerInterval);
                yield return new WaitForSeconds(healOverTimeInterval);
                elapsedTime += healOverTimeInterval;
            }
        }

        public void StartDamageOverTime(float frequency, float damagePerInterval, float duration)
        {
            StartCoroutine(DamageOverTime(frequency, damagePerInterval, duration));
        }

        public void StartHealOverTime(float frequency, float healthPerInterval, float duration)
        {
            StartCoroutine(HealOverTime(frequency, healthPerInterval, duration));
        }

        public void ToggleVisualFeedback(bool enable)
        {
            showVisualFeedback = enable;
        }
    }
}
