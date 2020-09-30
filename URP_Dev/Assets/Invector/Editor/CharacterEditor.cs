using Invector.CharacterController;
using UnityEditor;
using UnityEngine;

namespace Invector.Editors
{
	[CustomEditor(typeof(vThirdPersonMotor), true)]
	public class CharacterEditor : Editor
	{
		private GUISkin skin = default;
		private vThirdPersonMotor thirdPersonMotor = default;

		private void OnEnable()
		{
			thirdPersonMotor = (vThirdPersonMotor)target;
			skin = Resources.Load("GUISkin") as GUISkin;
		}

		public override void OnInspectorGUI()
		{
			if (!skin || !thirdPersonMotor)
			{
				return;
			}

			GUI.skin = skin;
			GUILayout.BeginVertical("BASIC THIRD PERSON CONTROLLER", "window");

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
