// HealthManager.cs

using UnityEngine;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    private static HealthManager instance;
    public static HealthManager Instance => instance;

    private Dictionary<GameObject, Health> healthDictionary = new Dictionary<GameObject, Health>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEntity(GameObject entity)
    {
        Health health = entity.GetComponent<Health>();

        if (health != null && !healthDictionary.ContainsKey(entity))
        {
            healthDictionary.Add(entity, health);
            health.OnHealthChanged += HandleHealthChanged;
        }
    }

    public void UnregisterEntity(GameObject entity)
    {
        if (healthDictionary.ContainsKey(entity))
        {
            healthDictionary[entity].OnHealthChanged -= HandleHealthChanged;
            healthDictionary.Remove(entity);
        }
    }

    private void HandleHealthChanged(float newHealth, float maxHealth)
    {
        // Implement any global health change logic here
    }
}
