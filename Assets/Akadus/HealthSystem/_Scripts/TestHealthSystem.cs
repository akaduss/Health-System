using UnityEngine;

public class TestHealthSystem : MonoBehaviour
{
    public GameObject player;

    public void Damage(float amount)
    {
        HealthManager.Instance.DamageObject(player, amount);
    }

    public void Heal(float amount)
    {
        HealthManager.Instance.HealObject(player, amount);
    }
}
