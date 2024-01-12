using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    private Dictionary<GameObject, Health> _healthDictionary;

    // Reference to the player's Health component
    [SerializeField] private Health _playerHealth;

    public Health PlayerHealth
    {
        get { return _playerHealth; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _healthDictionary = new Dictionary<GameObject, Health>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set the player's Health component reference
    public void SetPlayerHealth(Health playerHealth)
    {
        _playerHealth = playerHealth;
    }

    public void RegisterHealth(Health health)
    {
        if (!_healthDictionary.ContainsKey(health.gameObject))
        {
            _healthDictionary.Add(health.gameObject, health);
        }
    }

    public void UnregisterHealth(Health health)
    {
        if (_healthDictionary.ContainsKey(health.gameObject))
        {
            _healthDictionary.Remove(health.gameObject);
        }
    }

    public void DamageObject(GameObject target, float damage)
    {
        if (_healthDictionary.ContainsKey(target))
        {
            _healthDictionary[target].TakeDamage(damage);
        }
    }

    public void HealObject(GameObject target, float amount)
    {
        if (_healthDictionary.ContainsKey(target))
        {
            _healthDictionary[target].Heal(amount);
        }
    }

    // Add more methods as needed based on your requirements
}
