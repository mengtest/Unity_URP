using UnityEditor;
using UnityEngine;

namespace FPA
{
	public class DD_Menu : MonoBehaviour
	{
		[MenuItem("GameObject/UI/DataDiagram")]
		public static void AddDataDiagramInGameObject()
		{
			GameObject parent = null;
			if (Selection.activeTransform)
			{
				parent = Selection.activeTransform.gameObject;
			}
			else
			{
				parent = null;
			}

			if ((parent == null) || (parent.GetComponentInParent<Canvas>() == null))
			{
				Canvas canvas = FindObjectOfType<Canvas>();
				if (canvas == null)
				{
					Debug.LogError("AddDataDiagram : can not find a canvas in scene!");
					return;
				}
				else
				{
					parent = FindObjectOfType<Canvas>().gameObject;
				}
			}

			GameObject prefab = Resources.Load("Prefabs/DataDiagram") as GameObject;
			if (prefab == null)
			{
				Debug.LogError("AddDataDiagram : Load DataDiagram Error!");
				return;
			}

			GameObject dataDiagram;
			if (parent)
			{
				dataDiagram = Instantiate(prefab, parent.transform);
			}
			else
			{
				dataDiagram = Instantiate(prefab);
			}

			if (dataDiagram == null)
			{
				Debug.LogError("AddDataDiagram : Instantiate DataDiagram Error!");
				return;
			}

			Undo.RegisterCreatedObjectUndo(dataDiagram, "Created dataDiagram");
			dataDiagram.name = "DataDiagram";
		}
	}

}

