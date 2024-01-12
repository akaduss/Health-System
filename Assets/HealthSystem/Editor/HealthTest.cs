using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    private SerializedProperty damageAmountProp;
    private SerializedProperty healAmountProp;

    private void OnEnable()
    {
        damageAmountProp = serializedObject.FindProperty("damageAmount");
        healAmountProp = serializedObject.FindProperty("healAmount");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        GUILayout.Space(10);

        if (GUILayout.Button("Test Damage"))
        {
            ((Health)target).TakeDamage(damageAmountProp.floatValue);
        }

        if (GUILayout.Button("Test Heal"))
        {
            ((Health)target).Heal(healAmountProp.floatValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
