#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

[CustomEditor(typeof(WeaponManager))]
public class WeaponManagerEditor : Editor
{
    SerializedProperty m_weaponPositionProp;

    SerializedProperty m_ownerIsPlayerProp;
    SerializedProperty m_weaponCameraProp;
    SerializedProperty m_mainCameraProp;

    SerializedProperty m_primaryWeaponProp;
    SerializedProperty m_secondaryWeaponProp;


    private void OnEnable()
    {
        m_weaponPositionProp = serializedObject.FindProperty("weaponPosition");
       
        m_ownerIsPlayerProp = serializedObject.FindProperty("ownerIsPlayer");
        m_weaponCameraProp = serializedObject.FindProperty("weaponCamera");
        m_mainCameraProp = serializedObject.FindProperty("mainCamera");

        m_primaryWeaponProp = serializedObject.FindProperty("primaryWeapon");
        m_secondaryWeaponProp = serializedObject.FindProperty("secondaryWeapon");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_weaponPositionProp);
        EditorGUILayout.PropertyField(m_ownerIsPlayerProp);
        if (m_ownerIsPlayerProp.boolValue == true)
        {
            EditorGUILayout.PropertyField(m_weaponCameraProp);
            EditorGUILayout.PropertyField(m_mainCameraProp);
        }
        EditorGUILayout.PropertyField(m_primaryWeaponProp);
        EditorGUILayout.PropertyField(m_secondaryWeaponProp);


        serializedObject.ApplyModifiedProperties();
    }
}

#endif
