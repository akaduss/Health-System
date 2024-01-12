using UnityEngine;

public class TestHealthSystem : MonoBehaviour
{
    private Health _health;

    private void Start()
    {
        // Ensure the object has the Health script attached
        _health = GetComponent<Health>();

        if (_health == null)
        {
            Debug.LogError("Health script not found on the GameObject.");
            return;
        }

        // Subscribe to the OnHealthChanged event
        _health.OnHealthChanged += HandleHealthChanged;


    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnHealthChanged event to avoid memory leaks
        if (_health != null)
        {
            _health.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float newHealth, float maxHealth)
    {
        Debug.Log($"Health changed: Current Health = {newHealth}, Max Health = {maxHealth}");

        // Example: Check if the object is dead (health reached 0)
        if (newHealth <= 0f)
        {
            Debug.Log("The object is dead!");
        }
    }

}
