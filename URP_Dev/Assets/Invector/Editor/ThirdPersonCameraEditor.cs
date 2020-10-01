using UnityEditor;
using UnityEngine;
using TPS;

namespace Invector.Editors
{
	[CustomEditor(typeof(ThirdPersonCamera))]
	[CanEditMultipleObjects]
	public class ThirdPersonCameraEditor : Editor
	{
		private GUISkin skin = default;
		private ThirdPersonCamera tpCamera = default;

		private void OnSceneGUI()
		{
			if (Application.isPlaying)
			{
				return;
			}
			tpCamera = (ThirdPersonCamera)target;
		}

		private void OnEnable()
		{
			tpCamera = (ThirdPersonCamera)target;
			tpCamera.indexLookPoint = 0;
		}

		public override void OnInspectorGUI()
		{
			if (!skin)
			{
				skin = Resources.Load("GUISkin") as GUISkin;
			}
			GUI.skin = skin;
			GUILayout.BeginVertical("BASIC THIRD PERSON CAMERA", "window");

			{
				GUILayout.Space(30);
				EditorGUILayout.BeginVertical();
				base.OnInspectorGUI();
				GUILayout.Space(10);
				EditorGUILayout.EndVertical();
			}

			GUILayout.EndVertical();
			GUILayout.Space(2);
		}
	}
}