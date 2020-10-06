using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPA
{

	public class DrawLine : MonoBehaviour
	{
		[SerializeField]
		private DataDiagram dataDiagram = default;

		private List<GameObject> lineList = new List<GameObject>();
		private float time = 0f;
		private float h = 0;


		private void AddALine()
		{
			if (dataDiagram == null)
			{
				return;
			}

			Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
			GameObject line = dataDiagram.AddLine(color.ToString(), color);
			if (line)
			{
				lineList.Add(line);
			}
		}

		private void Start()
		{
			if (dataDiagram == null)
			{
				return;
			}
			dataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };
			AddALine();
		}

		private void FixedUpdate()
		{
			if (dataDiagram == null)
			{
				return;
			}

			time += Time.deltaTime;
			ContinueInput(time);
		}

		private void ContinueInput(float f)
		{
			float d = 0f;
			foreach (GameObject l in lineList)
			{
				dataDiagram.InputPoint(l, new Vector2(0.1f, (Mathf.Sin(f + d) + 1f) * 2f));
				d += 1f;
			}
		}


	}

}

