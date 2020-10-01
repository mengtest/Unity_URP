using TPS;
using UnityEditor;
using UnityEngine;

namespace Invector.Editors
{
	[CustomEditor(typeof(ThirdPersonMotor), true)]
	public class CharacterEditor : Editor
	{
		private GUISkin skin = default;
		private ThirdPersonMotor thirdPersonMotor = default;

		private void OnEnable()
		{
			thirdPersonMotor = (ThirdPersonMotor)target;
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
