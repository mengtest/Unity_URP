using UnityEditor;
using UnityEngine;

namespace Invector.vCamera
{
    [CustomEditor(typeof(vThirdPersonCamera))]
    [CanEditMultipleObjects]
    public class vThirdPersonCameraEditor : Editor
    {
        GUISkin skin;
        vThirdPersonCamera tpCamera;

        void OnSceneGUI()
        {
            if (Application.isPlaying)
                return;
            tpCamera = (vThirdPersonCamera)target;
        }

        void OnEnable()
        {
            tpCamera = (vThirdPersonCamera)target;
            tpCamera.indexLookPoint = 0;
        }

        public override void OnInspectorGUI()
        {
            if (!skin) skin = Resources.Load("GUISkin") as GUISkin;
            GUI.skin = skin;

            GUILayout.BeginVertical("BASIC THIRD PERSON CAMERA", "window");

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
}