using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPA
{

	public class Sample : MonoBehaviour
	{
		[SerializeField]
		private DD_DataDiagram dataDiagram = default;

		private List<GameObject> lineList = new List<GameObject>();
		private bool isContinueInput = false;
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
			GameObject dd = GameObject.Find("DataDiagram");
			if (dd == null)
			{
				Debug.LogWarning("can not find a gameobject of DataDiagram");
				return;
			}
			dataDiagram = dd.GetComponent<DD_DataDiagram>();
			dataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };
			AddALine();
		}

		private void FixedUpdate()
		{
			time += Time.deltaTime;
			ContinueInput(time);
		}

		private void ContinueInput(float f)
		{
			if (dataDiagram == null)
			{
				return;
			}

			if (isContinueInput == false)
			{
				return;
			}

			float d = 0f;
			foreach (GameObject l in lineList)
			{
				dataDiagram.InputPoint(l, new Vector2(0.1f, (Mathf.Sin(f + d) + 1f) * 2f));
				d += 1f;
			}
		}

		public void OnClickRamdomValue()
		{
			if (dataDiagram == null)
			{
				return;
			}
			foreach (GameObject l in lineList)
			{
				dataDiagram.InputPoint(l, new Vector2(1, Random.value * 4f));
			}
		}

		public void OnClickAddLine()
		{
			AddALine();
		}

		public void OnClickRectChange()
		{
			if (dataDiagram == null)
			{
				return;
			}
			Rect rect = new Rect(Random.value * Screen.width, Random.value * Screen.height, Random.value * Screen.width / 2, Random.value * Screen.height / 2);
			dataDiagram.rect = rect;
		}

		public void OnClickContinueInput()
		{
			isContinueInput = !isContinueInput;
		}

	}
}

