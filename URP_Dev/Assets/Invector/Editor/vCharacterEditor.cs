using Invector.vCharacterController;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(vThirdPersonMotor), true)]
public class vCharacterEditor : Editor
{
    private GUISkin skin = default;
    private vThirdPersonMotor motor = default;


    private void OnEnable()
    {
        motor = (vThirdPersonMotor)target;
        skin = Resources.Load("GUISkin") as GUISkin;
    }


    public override void OnInspectorGUI()
    {
        if (!skin || !motor)
        {
            return;
        }

        GUI.skin = skin;
        GUILayout.BeginVertical("BASIC THIRD PERSON CONTROLLER", "window");
        GUILayout.Space(30);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        base.OnInspectorGUI();
        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}