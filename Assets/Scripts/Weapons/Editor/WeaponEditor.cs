using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{

    SerializedProperty m_WeaponStatsProp;
    SerializedProperty m_TriggerTypeProp;
    SerializedProperty m_WeaponClassProp;
    SerializedProperty m_WeaponTypeProp;

    SerializedProperty m_ProjectilePrefabProp;
    SerializedProperty m_ProjectileLaunchForceProp;
    
    SerializedProperty m_EndPointProp;
    SerializedProperty m_AdvancedSettingsProp;

    SerializedProperty m_FireAnimationClipProp;
    SerializedProperty m_ReloadAnimationClipProp;

    SerializedProperty m_FireAudioClipProp;
    SerializedProperty m_ReloadAudioClipProp;

    SerializedProperty m_PrefabRayTrailProp;
    SerializedProperty m_DisabledOnEmpty;

    void OnEnable()
    {

        m_WeaponStatsProp = serializedObject.FindProperty("weaponStats");
        m_TriggerTypeProp = serializedObject.FindProperty("triggerType");
        m_WeaponClassProp = serializedObject.FindProperty("weaponClass");
        m_WeaponTypeProp = serializedObject.FindProperty("weaponType");
        
        m_ProjectilePrefabProp = serializedObject.FindProperty("projectilePrefab");
        m_ProjectileLaunchForceProp = serializedObject.FindProperty("projectileLaunchForce");

        m_EndPointProp = serializedObject.FindProperty("endPoint");
        m_AdvancedSettingsProp = serializedObject.FindProperty("advancedSettings");

        m_FireAnimationClipProp = serializedObject.FindProperty("fireAnimationClip");
        m_ReloadAnimationClipProp = serializedObject.FindProperty("reloadAnimationClip");
        m_FireAudioClipProp = serializedObject.FindProperty("fireAudioClip");
        m_ReloadAudioClipProp = serializedObject.FindProperty("reloadAudioClip");
        m_PrefabRayTrailProp = serializedObject.FindProperty("prefabRayTrail");

        m_DisabledOnEmpty = serializedObject.FindProperty("disabledOnEmpty");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUILayout.PropertyField(m_WeaponStatsProp);
        EditorGUILayout.PropertyField(m_TriggerTypeProp);
        EditorGUILayout.PropertyField(m_WeaponClassProp);
        EditorGUILayout.PropertyField(m_WeaponTypeProp);

        if (m_WeaponTypeProp.intValue == (int)Weapon.WeaponType.Projectile)
        {
            EditorGUILayout.PropertyField(m_ProjectilePrefabProp);
            EditorGUILayout.PropertyField(m_ProjectileLaunchForceProp);
        }

        EditorGUILayout.PropertyField(m_EndPointProp);
        EditorGUILayout.PropertyField(m_DisabledOnEmpty);
        EditorGUILayout.PropertyField(m_AdvancedSettingsProp, new GUIContent("Advance Settings"), true);
        EditorGUILayout.PropertyField(m_FireAnimationClipProp);
        EditorGUILayout.PropertyField(m_ReloadAnimationClipProp);
        EditorGUILayout.PropertyField(m_FireAudioClipProp);
        EditorGUILayout.PropertyField(m_ReloadAudioClipProp);

        if (m_WeaponTypeProp.intValue == (int)Weapon.WeaponType.Raycast)
        {
            EditorGUILayout.PropertyField(m_PrefabRayTrailProp);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
#endif
