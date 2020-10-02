using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FPA
{

	public class DataDiagram : MonoBehaviour, IScrollHandler, IDragHandler
	{
		public delegate void RectChangeHandler(object sender, RectChangeEventArgs e);
		public delegate void ZoomHandler(object sender, ZoomEventArgs e);
		public delegate void MoveHandler(object sender, MoveEventArgs e);
		public delegate void PreDestroyLineHandler(object sender, PreDestroyLineEventArgs e);
		public event RectChangeHandler RectChangeEvent = default;
		public event ZoomHandler ZoomEvent = default;
		public event MoveHandler MoveEvent = default;
		public event PreDestroyLineHandler PreDestroyLineEvent = default;

		private readonly Vector2 MinRectSize = new Vector2(100, 80);

		private GameObject coordinateAxisGO = default;
		private GameObject lineButtonsContent = default;

		public int maxLineNum = 5;
		public int maxPointNum = 65535;

		public float CentimeterPerMark = 1f;
		public float CentimeterPerCoordUnitX = 1f;
		public float CentimeterPerCoordUnitY = 1f;

		public Rect? Rect
		{
			get
			{
				RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
				if (rectTransform == null)
				{
					return null;
				}
				return rectTransform.rect;
			}
			set
			{
				Rect rect = value.Value;
				if (MinRectSize.x > rect.width)
				{
					rect.width = MinRectSize.x;
				}
				if (MinRectSize.y > rect.height)
				{
					rect.height = MinRectSize.y;
				}

				RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
				if (rectTransform == null)
				{
					return;
				}

				var parentRectTrans = transform.parent.GetComponentInParent<RectTransform>();
				rectTransform.anchoredPosition = CalcRectTransformHelper.CalcAnchorPosition(rect, rectTransform.anchorMin, rectTransform.anchorMax, parentRectTrans.rect.size, rectTransform.pivot);
				rectTransform.sizeDelta = CalcRectTransformHelper.CalcSizeDelta(rect, rectTransform.anchorMin, rectTransform.anchorMax, parentRectTrans.rect.size);
				RectChangeEvent(this, new RectChangeEventArgs(rect.size));
			}
		}

		private void Awake()
		{
			CoordinateAxis coordinateAxis = transform.GetComponentInChildren<CoordinateAxis>();
			if (coordinateAxis == null)
			{
				coordinateAxisGO = Instantiate((GameObject)Resources.Load("Prefabs/CoordinateAxis"), gameObject.transform);
				coordinateAxisGO.name = "CoordinateAxis";
			}
			else
			{
				coordinateAxisGO = coordinateAxis.gameObject;
			}

			LineButtonsContent tempObject = GetComponentInChildren<LineButtonsContent>();
			if (tempObject == null)
			{
				Debug.LogWarning(this + "Awake Error : null == lineButtonsContent");
				return;
			}
			else
			{
				if ((lineButtonsContent = tempObject.gameObject) == null)
				{
					Debug.LogWarning(this + "Awake Error : null == lineButtonsContent");
					return;
				}
			}
		}

		private void Start()
		{
			if (RectChangeEvent != null)
			{
				try
				{
					RectChangeEvent(this, new RectChangeEventArgs(gameObject.GetComponent<RectTransform>().rect.size));
				}
				catch (NullReferenceException e)
				{
					Debug.LogWarning(e);
				}
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			MoveEvent(this, new MoveEventArgs(eventData.delta.x, eventData.delta.y));
		}

		public void OnScroll(PointerEventData eventData)
		{
			if (Input.GetMouseButton(0))
			{
				ZoomEvent(this, new ZoomEventArgs(-eventData.scrollDelta.y, 0));
			}
			else if (Input.GetMouseButton(1))
			{
				ZoomEvent(this, new ZoomEventArgs(0, eventData.scrollDelta.y));
			}
			else
			{
				ZoomEvent(this, new ZoomEventArgs(-eventData.scrollDelta.y, -eventData.scrollDelta.y));
			}
		}

		private void SetLineButtonColor(GameObject line, Color color)
		{
			foreach (Transform t in lineButtonsContent.transform)
			{
				if (line == t.gameObject.GetComponent<LineButton>().Line)
				{
					t.gameObject.GetComponent<LineButton>().Line = line;
					return;
				}
			}
		}

		private void SetLineColor(GameObject line, Color color)
		{
			if (!line)
			{
				return;
			}

			DrawGraphicLines lines = line.GetComponent<DrawGraphicLines>();
			if (!lines)
			{
				Debug.LogWarning(line.ToString() + " SetLineColor error : null == lines");
				return;
			}
			lines.color = color;
			SetLineButtonColor(line, color);
		}

		private bool AddLineButton(GameObject line)
		{
			if (!lineButtonsContent)
			{
				Debug.LogWarning(this + "AddLineButton Error : null == lineButtonsContent");
				return false;
			}

			if (lineButtonsContent.transform.childCount >= maxLineNum)
			{
				return false;
			}

			if (!line)
			{
				Debug.LogWarning(this + "AddLineButton Error : null == line");
				return false;
			}

			DrawGraphicLines lines = line.GetComponent<DrawGraphicLines>();
			if (!lines)
			{
				Debug.LogWarning(this + "AddLineButton Error : null == lines");
				return false;
			}

			GameObject button = Instantiate((GameObject)Resources.Load("Prefabs/LineButton"), lineButtonsContent.transform);
			if (!button)
			{
				Debug.LogWarning(this + "AddLineButton Error : null == button");
				return false;
			}
			button.GetComponent<LineButton>().Line = line;
			return true;
		}

		private bool DestroyLineButton(GameObject line)
		{
			if (!lineButtonsContent)
			{
				Debug.Log(this + "AddLineButton Error : null == lineButtonsContent");
				return false;
			}

			foreach (Transform t in lineButtonsContent.transform)
			{
				try
				{
					if (line == t.gameObject.GetComponent<LineButton>().Line)
					{
						t.gameObject.GetComponent<LineButton>().DestroyLineButton();
						Destroy(t.gameObject);
						return true;
					}
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}

			return false;
		}

		public void InputPoint(GameObject line, Vector2 point)
		{

			CoordinateAxis coordinate = coordinateAxisGO.GetComponent<CoordinateAxis>();
			coordinate.InputPoint(line, point);
		}

		public GameObject AddLine(string name)
		{
			CoordinateAxis coordinate = coordinateAxisGO.GetComponent<CoordinateAxis>();

			if (coordinate.lineNum >= maxLineNum)
			{
				Debug.Log("coordinate.lineNum > maxLineNum");
				return null;
			}

			if (coordinate.lineNum != lineButtonsContent.transform.childCount)
			{
				Debug.Log("coordinate.lineNum != m_LineButtonList.Count");
			}

			GameObject line = coordinate.AddLine(name);
			if (!AddLineButton(line))
			{
				if (!coordinate.RemoveLine(line))
				{
					Debug.Log(this.ToString() + " AddLine error : false == coordinate.RemoveLine(line)");
				}
				line = null;
			}
			return line;
		}

		public GameObject AddLine(string name, Color color)
		{
			GameObject line = AddLine(name);
			SetLineColor(line, color);
			return line;
		}

		public bool DestroyLine(GameObject line)
		{
			PreDestroyLineEvent(this, new PreDestroyLineEventArgs(line));

			if (!DestroyLineButton(line))
			{
				return false;
			}

			try
			{
				if (!coordinateAxisGO.GetComponent<CoordinateAxis>().RemoveLine(line))
				{
					return false;
				}
			}
			catch (NullReferenceException)
			{
				return false;
			}

			return true;
		}
	}

}


