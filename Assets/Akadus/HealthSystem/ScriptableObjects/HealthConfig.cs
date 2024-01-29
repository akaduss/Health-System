using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "ScriptableObjects/HealthConfig")]
public class HealthConfig : ScriptableObject
{
    public float maxHealth = 100f;
    public float damageFeedbackDuration = 0.2f;
    public float healFeedbackDuration = 0.2f;
    public float invulnerabilityDuration = 1f;
    public Color damageColor = Color.red;
    public Color healColor = Color.green;
}
