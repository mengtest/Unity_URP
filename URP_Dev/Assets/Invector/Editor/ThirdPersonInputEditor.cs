using UnityEditor;
using UnityEngine;

namespace Invector.Editors
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(CharacterController.vThirdPersonInput), true)]
	public class ThirdPersonInputEditor : Editor
	{
		private GUISkin skin = default;

		public override void OnInspectorGUI()
		{
			if (!skin)
			{
				skin = Resources.Load("GUISkin") as GUISkin;
			}
			GUI.skin = skin;

			GUILayout.BeginVertical("BASIC THIRD PERSON INPUT", "window");

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

