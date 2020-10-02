using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FPA
{
	public class DD_CalcRectTransformHelper
	{

		public static Vector2 CalcAnchorPointPosition(Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize, Vector2 pivot)
		{
			Vector2 pos = new Vector2(parentSize.x * anchorMin.x, parentSize.y * anchorMin.y);
			Vector2 size = new Vector2(parentSize.x * anchorMax.x - pos.x, parentSize.y * anchorMax.y - pos.y);
			return pos + new Vector2(size.x * pivot.x, size.y * pivot.y);
		}

		public static Vector2 CalcAnchorPosition(Rect rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize, Vector2 pivot)
		{
			Vector2 anchor = CalcAnchorPointPosition(anchorMin, anchorMax, parentSize, pivot);
			Vector2 pivotPos = new Vector2(rect.x + rect.width * pivot.x, rect.y + rect.height * pivot.y);
			return pivotPos - anchor;
		}

		public static Vector2 CalcOffsetMin(Rect rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize)
		{
			Vector2 anchor0 = new Vector2(parentSize.x * anchorMin.x, parentSize.y * anchorMin.y);
			Vector2 point0 = new Vector2(rect.x, rect.y);
			return point0 - anchor0;
		}

		public static Vector2 CalcOffsetMax(Rect rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize)
		{
			Vector2 anchor2 = new Vector2(parentSize.x * anchorMax.x, parentSize.y * anchorMax.y);
			Vector2 point2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			return point2 - anchor2;
		}

		public static Vector2 CalcSizeDelta(Rect rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize)
		{
			return (CalcOffsetMax(rect, anchorMin, anchorMax, parentSize) - CalcOffsetMin(rect, anchorMin, anchorMax, parentSize));
		}

		public static Vector2 CalcRectSize(Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize)
		{
			Vector2 anchor0 = new Vector2(parentSize.x * anchorMin.x, parentSize.y * anchorMin.y);
			Vector2 anchor2 = new Vector2(parentSize.x * anchorMax.x, parentSize.y * anchorMax.y);
			return anchor2 - anchor0 + sizeDelta;
		}

		public static Rect CalcLocalRect(Vector2 anchorMin, Vector2 anchorMax, Vector2 parentSize, Vector2 pivot, Vector2 anchorPosition, Rect rectInRT)
		{
			Vector2 anchor = CalcAnchorPointPosition(anchorMin, anchorMax, parentSize, pivot);
			Vector2 pivotPos = anchor + anchorPosition;
			return new Rect(pivotPos + rectInRT.position, rectInRT.size);
		}
	}

	public class DD_RectChangeEventArgs : EventArgs
	{
		private readonly Vector2 m_Size;

		public DD_RectChangeEventArgs(Vector2 size)
		{
			m_Size = size;
		}

		public Vector2 size { get => m_Size; }
	}

	public class DD_ZoomEventArgs : EventArgs
	{
		private float _zoomX;
		private float _zoomY;

		public DD_ZoomEventArgs(float valX, float valY) : base()
		{
			_zoomX = valX;
			_zoomY = valY;
		}

		public float ZoomX { get => _zoomX; }
		public float ZoomY { get => _zoomY; }
	}

	public class DD_MoveEventArgs : EventArgs
	{
		private float _moveX = 0;
		private float _moveY = 0;

		public DD_MoveEventArgs(float dx, float dy)
		{
			_moveX = dx;
			_moveY = dy;
		}

		public float MoveX { get => _moveX; }
		public float MoveY { get => _moveY; }
	}

	public class DD_PreDestroyLineEventArgs : EventArgs
	{

		GameObject m_Line = null;

		public DD_PreDestroyLineEventArgs(GameObject line)
		{
			m_Line = null;

			if (!line)
			{
				return;
			}

			if (!line.GetComponent<DD_Lines>())
			{
				return;
			}
			m_Line = line;
		}

		public GameObject line { get => m_Line; }
	}

	public class DD_DataDiagram : MonoBehaviour, IScrollHandler, IDragHandler
	{
		public delegate void RectChangeHandler(object sender, DD_RectChangeEventArgs e);
		public delegate void ZoomHandler(object sender, DD_ZoomEventArgs e);
		public delegate void MoveHandler(object sender, DD_MoveEventArgs e);
		public delegate void PreDestroyLineHandler(object sender, DD_PreDestroyLineEventArgs e);
		public event RectChangeHandler RectChangeEvent;
		public event ZoomHandler ZoomEvent;
		public event MoveHandler MoveEvent;
		public event PreDestroyLineHandler PreDestroyLineEvent;

		private readonly Vector2 MinRectSize = new Vector2(100, 80);

		private GameObject m_CoordinateAxis;
		private GameObject lineButtonsContent;

		public int maxLineNum = 5;
		public int m_MaxPointNum = 65535;

		public float m_CentimeterPerMark = 1f;

		public float m_CentimeterPerCoordUnitX = 1f;

		public float m_CentimeterPerCoordUnitY = 1f;

		public Rect? rect
		{
			get
			{
				RectTransform rectT = gameObject.GetComponent<RectTransform>();
				if (null == rectT)
				{
					return null;
				}
				return rectT.rect;
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

				RectTransform rectT = gameObject.GetComponent<RectTransform>();
				if (null == rectT)
				{
					return;
				}

				rectT.anchoredPosition = DD_CalcRectTransformHelper.CalcAnchorPosition(rect, rectT.anchorMin, rectT.anchorMax, transform.parent.GetComponentInParent<RectTransform>().rect.size, rectT.pivot);
				rectT.sizeDelta = DD_CalcRectTransformHelper.CalcSizeDelta(rect, rectT.anchorMin, rectT.anchorMax, transform.parent.GetComponentInParent<RectTransform>().rect.size);
				RectChangeEvent(this, new DD_RectChangeEventArgs(rect.size));
			}
		}

		private void Awake()
		{
			DD_CoordinateAxis coordinateAxis = transform.GetComponentInChildren<DD_CoordinateAxis>();
			if (coordinateAxis == null)
			{
				m_CoordinateAxis = Instantiate((GameObject)Resources.Load("Prefabs/CoordinateAxis"), gameObject.transform);
				m_CoordinateAxis.name = "CoordinateAxis";
			}
			else
			{
				m_CoordinateAxis = coordinateAxis.gameObject;
			}

			DD_LineButtonsContent tempObject = GetComponentInChildren<DD_LineButtonsContent>();
			if (tempObject == null)
			{
				Debug.LogWarning(this + "Awake Error : null == lineButtonsContent");
				return;
			}
			else
			{
				if (null == (lineButtonsContent = tempObject.gameObject))
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
					RectChangeEvent(this, new DD_RectChangeEventArgs(gameObject.GetComponent<RectTransform>().rect.size));
				}
				catch (NullReferenceException e)
				{
					Debug.LogWarning(e);
				}
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			MoveEvent(this, new DD_MoveEventArgs(eventData.delta.x, eventData.delta.y));
		}

		public void OnScroll(PointerEventData eventData)
		{
			if (Input.GetMouseButton(0))
			{
				ZoomEvent(this, new DD_ZoomEventArgs(-eventData.scrollDelta.y, 0));
			}
			else if (Input.GetMouseButton(1))
			{
				ZoomEvent(this, new DD_ZoomEventArgs(0, eventData.scrollDelta.y));
			}
			else
			{
				ZoomEvent(this, new DD_ZoomEventArgs(-eventData.scrollDelta.y, -eventData.scrollDelta.y));
			}
		}

		private void SetLineButtonColor(GameObject line, Color color)
		{
			foreach (Transform t in lineButtonsContent.transform)
			{
				if (line == t.gameObject.GetComponent<DD_LineButton>().Line)
				{
					t.gameObject.GetComponent<DD_LineButton>().Line = line;
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

			DD_Lines lines = line.GetComponent<DD_Lines>();
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

			DD_Lines lines = line.GetComponent<DD_Lines>();
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
			button.GetComponent<DD_LineButton>().Line = line;
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
					if (line == t.gameObject.GetComponent<DD_LineButton>().Line)
					{
						t.gameObject.GetComponent<DD_LineButton>().DestroyLineButton();
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

			DD_CoordinateAxis coordinate = m_CoordinateAxis.GetComponent<DD_CoordinateAxis>();
			coordinate.InputPoint(line, point);
		}

		public GameObject AddLine(string name)
		{

			DD_CoordinateAxis coordinate = m_CoordinateAxis.GetComponent<DD_CoordinateAxis>();

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
			PreDestroyLineEvent(this, new DD_PreDestroyLineEventArgs(line));

			if (!DestroyLineButton(line))
			{
				return false;
			}

			try
			{
				if (!m_CoordinateAxis.GetComponent<DD_CoordinateAxis>().RemoveLine(line))
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


