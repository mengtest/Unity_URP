using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace FPA
{

	public class CoordinateAxis : DiagramDrawGraphic
	{
		private static readonly string MARK_TEXT_BASE_NAME = "MarkText";
		private static readonly string LINES_BASE_NAME = "Line";
		private static readonly string COORDINATE_RECT = "CoordinateRect";
		private const float INCH_PER_CENTIMETER = 0.3937008f;
		private readonly float[] MarkIntervalTab = { 1, 2, 5 };

		private DataDiagram dataDiagram = default;
		private RectTransform coordinateRectTransform = default;
		private GameObject linesPrefab = default;
		private GameObject markTextPrefab = default;

		private List<GameObject> lineList = new List<GameObject>();
		private Vector2 zoomSpeed = new Vector2(1, 1);
		private Vector2 moveSpeed = new Vector2(1, 1);

		private float coordinateAxisMaxWidth = 100;
		private float coordinateAxisMinWidth = 0.1f;
		private float rectThickness = 2;

		private Color backgroundColor = new Color(0, 0, 0, 0.5f);
		private Color markColor = new Color(0.8f, 0.8f, 0.8f, 1);

		private List<GameObject> m_MarkHorizontalTexts = new List<GameObject>();

		private float m_MinMarkTextHeight = 20;

		private float m_PixelPerMark
		{
			get => dataDiagram ? INCH_PER_CENTIMETER * dataDiagram.CentimeterPerMark * Screen.dpi : 0f;
		}

		private Rect coordinateAxisRange
		{
			get
			{
				try
				{
					Vector2 sizePixel = coordinateRectTransform.rect.size;
					return new Rect(0, 0,
						sizePixel.x / (dataDiagram.CentimeterPerCoordUnitX * INCH_PER_CENTIMETER * Screen.dpi),
						sizePixel.y / (dataDiagram.CentimeterPerCoordUnitY * INCH_PER_CENTIMETER * Screen.dpi));
				}
				catch (NullReferenceException e)
				{
					Debug.Log(this + " : " + e);
				}
				return new Rect(Vector2.zero, GetComponent<RectTransform>().rect.size);
			}
		}

		private Rect coordinateAxisViewRange = new Rect(1, 1, 1, 1);

		private float coordinateAxisViewSizeX
		{
			get
			{
				try
				{
					return coordinateAxisRange.width * coordinateAxisViewRange.width;
				}
				catch (NullReferenceException e)
				{
					Debug.Log(this + " : " + e);
				}
				return coordinateAxisRange.width;
			}
		}

		private float coordinateAxisViewSizeY
		{
			get
			{
				try
				{
					return coordinateAxisRange.height * coordinateAxisViewRange.height;
				}
				catch (NullReferenceException e)
				{
					Debug.Log(this + " : " + e);
				}
				return coordinateAxisRange.width;
			}
		}

		public Rect coordinateAxisViewRangeInPixel
		{
			get
			{
				try
				{
					return new Rect(CoordinateToPixel(coordinateAxisViewRange.position - coordinateAxisRange.position), coordinateAxisViewRange.size);
				}
				catch (NullReferenceException e)
				{
					Debug.Log(this + " : " + e);
				}
				return new Rect(CoordinateToPixel(coordinateAxisRange.position), coordinateAxisViewRange.size);
			}
		}

		public RectTransform coordinateRectT
		{
			get
			{
				try
				{
					return coordinateRectTransform;
				}
				catch
				{
					return GetComponent<RectTransform>();
				}
			}
		}

		public int lineNum
		{
			get => lineList.Count;
		}

		public delegate void CoordinateRectChangeHandler(object sender, CoordinateRectChangeEventArgs e);
		public delegate void CoordinateScaleChangeHandler(object sender, CoordinateScaleChangeEventArgs e);
		public delegate void CoordinateZeroPointChangeHandler(object sender, CoordinateZeroPointChangeEventArgs e);
		public event CoordinateRectChangeHandler CoordinateRectChangeEvent;
		public event CoordinateScaleChangeHandler CoordinateScaleChangeEvent;
		public event CoordinateZeroPointChangeHandler CoordinateeZeroPointChangeEvent;

		protected override void Awake()
		{
			dataDiagram = GetComponentInParent<DataDiagram>();
			if (!dataDiagram)
			{
				Debug.Log(this + "Awake Error : null == m_DataDiagram");
				return;
			}

			linesPrefab = (GameObject)Resources.Load("Prefabs/Lines");
			if (linesPrefab == null)
			{
				Debug.Log("Error : null == m_LinesPreb");
			}

			markTextPrefab = (GameObject)Resources.Load("Prefabs/MarkText");
			if (markTextPrefab == null)
			{
				Debug.Log("Error : null == m_MarkTextPreb");
			}

			try
			{
				coordinateRectTransform = FindInChild(COORDINATE_RECT).GetComponent<RectTransform>();
				if (null == coordinateRectTransform)
				{
					Debug.Log("Error : null == m_CoordinateRectT");
					return;
				}
			}
			catch (NullReferenceException e)
			{
				Debug.Log(this + "," + e);
			}

			FindExistMarkText(m_MarkHorizontalTexts);
			GameObject parent = gameObject.transform.parent.gameObject;
			Rect parentRect = parent.GetComponent<RectTransform>().rect;
			coordinateAxisViewRange.position = coordinateAxisRange.position;
			coordinateAxisViewRange.size = new Vector2(1, 1);
			dataDiagram.RectChangeEvent += OnRectChange;
			dataDiagram.ZoomEvent += OnZoom;
			dataDiagram.MoveEvent += OnMove;
		}


		private GameObject FindInChild(string name)
		{
			foreach (Transform t in transform)
			{
				if (name == t.gameObject.name)
				{
					return t.gameObject;
				}
			}

			return null;
		}

		private void ChangeRect(Rect newRect)
		{
			if (CoordinateRectChangeEvent != null)
			{
				CoordinateRectChangeEvent(this, new CoordinateRectChangeEventArgs(new Rect(CoordinateToPixel(coordinateAxisRange.position - coordinateAxisViewRange.position), newRect.size)));
			}
		}

		private void ChangeScale(float ZoomX, float ZoomY)
		{

			Vector2 rangeSize = coordinateAxisRange.size;
			Vector2 viewSize = new Vector2(coordinateAxisViewRange.width * rangeSize.x, coordinateAxisViewRange.height * rangeSize.y);

			float YtoXScale = (rangeSize.y / rangeSize.x);
			float zoomXVal = ZoomX * zoomSpeed.x;
			float zoomYVal = (ZoomY * zoomSpeed.y) * YtoXScale;

			viewSize.x += zoomXVal;
			viewSize.y += zoomYVal;

			if (viewSize.x > coordinateAxisMaxWidth)
			{
				viewSize.x = coordinateAxisMaxWidth;
			}

			if (viewSize.x < coordinateAxisMinWidth)
			{
				viewSize.x = coordinateAxisMinWidth;
			}

			if (viewSize.y > coordinateAxisMaxWidth * YtoXScale)
			{
				viewSize.y = coordinateAxisMaxWidth * YtoXScale;
			}

			if (viewSize.y < coordinateAxisMinWidth * YtoXScale)
			{
				viewSize.y = coordinateAxisMinWidth * YtoXScale;
			}

			coordinateAxisViewRange.width = viewSize.x / rangeSize.x;
			coordinateAxisViewRange.height = viewSize.y / rangeSize.y;
		}

		private void OnRectChange(object sender, RectChangeEventArgs e)
		{
			ChangeRect(coordinateRectTransform.rect);
			UpdateGeometry();
		}

		private void OnZoom(object sender, ZoomEventArgs e)
		{
			if (CoordinateScaleChangeEvent != null)
			{
				CoordinateScaleChangeEvent(this, new CoordinateScaleChangeEventArgs(coordinateAxisViewRange.width, coordinateAxisViewRange.height));
			}
			ChangeScale(e.ZoomX, e.ZoomY);
			UpdateGeometry();
		}

		private void OnMove(object sender, MoveEventArgs e)
		{

			if ((1 > Mathf.Abs(e.MoveX)) && (1 > Mathf.Abs(e.MoveY)))
			{
				return;
			}

			Vector2 coordDis = new Vector2(
				(e.MoveX / coordinateRectTransform.rect.width) * coordinateAxisViewSizeX,
				(e.MoveY / coordinateRectTransform.rect.height) * coordinateAxisViewSizeY);

			Vector2 dis = new Vector2(-coordDis.x * moveSpeed.x, -coordDis.y * moveSpeed.y);

			coordinateAxisViewRange.position += dis;
			if (0 > coordinateAxisViewRange.x)
			{
				coordinateAxisViewRange.x = 0;
			}

			if (CoordinateeZeroPointChangeEvent != null)
			{
				CoordinateeZeroPointChangeEvent(this, new CoordinateZeroPointChangeEventArgs(CoordinateToPixel(dis)));
			}
			UpdateGeometry();
		}


		private Vector2 CoordinateToPixel(Vector2 coordPoint)
		{
			return new Vector2((coordPoint.x / coordinateAxisRange.width) * coordinateRectTransform.rect.width, (coordPoint.y / coordinateAxisRange.height) * coordinateRectTransform.rect.height);
		}

		private int CalcMarkNum(float pixelPerMark, float totalPixel)
		{
			return Mathf.CeilToInt(totalPixel / (pixelPerMark > 0 ? pixelPerMark : 1));
		}

		private float CalcMarkLevel(float[] makeTab, int markNum, float viewMarkRange)
		{
			float dis = viewMarkRange / (markNum > 0 ? markNum : 1);
			float markScale = 1;
			float mark = makeTab[0];

			while ((dis < (mark * markScale)) || (dis >= (mark * markScale * 10)))
			{

				if (dis < (mark * markScale))
				{
					markScale /= 10;
				}
				else if (dis >= (mark * markScale * 10))
				{
					markScale *= 10;
				}
				else
				{
					break;
				}
			}

			dis /= markScale;
			for (int i = 1; i < makeTab.Length; i++)
			{
				if (Mathf.Abs(mark - dis) > Mathf.Abs(makeTab[i] - dis))
					mark = makeTab[i];
			}

			return (mark * markScale);
		}

		private float CeilingFormat(float markLevel, float Val)
		{
			return Mathf.CeilToInt(Val / markLevel) * markLevel;
		}

		private float[] CalcMarkVals(float markLevel, float startViewMarkVal, float endViewMarkVal)
		{
			float[] markVals;
			List<float> tempList = new List<float>();
			float tempMarkVal = CeilingFormat(markLevel, startViewMarkVal);

			while (tempMarkVal < endViewMarkVal)
			{
				tempList.Add(tempMarkVal);
				tempMarkVal += markLevel;
			}

			markVals = new float[tempList.Count];
			tempList.CopyTo(markVals);

			return markVals;
		}

		private float MarkValToPixel(float markVal, float startViewMarkVal, float endViewMarkVal, float stratCoordPixelVal, float endCoordPixelVal)
		{
			if ((endViewMarkVal <= startViewMarkVal) || (markVal <= startViewMarkVal))
			{
				return stratCoordPixelVal;
			}
			return stratCoordPixelVal + ((endCoordPixelVal - stratCoordPixelVal) * ((markVal - startViewMarkVal) / (endViewMarkVal - startViewMarkVal)));
		}

		private float[] MarkValsToPixel(float[] markVals, float startViewMarkVal, float endViewMarkVal, float stratCoordPixelVal, float endCoordPixelVal)
		{
			float[] pixelYs = new float[markVals.Length];

			for (int i = 0; i < pixelYs.Length; i++)
			{
				pixelYs[i] = MarkValToPixel(markVals[i], startViewMarkVal, endViewMarkVal, stratCoordPixelVal, endCoordPixelVal);
			}
			return pixelYs;
		}

		private void SetMarkText(GameObject markText, Rect rect, string str, bool isEnable)
		{
			if (markText == null)
			{
				Debug.Log("SetMarkText Error : null == markText");
				return;
			}

			RectTransform rectTransform = markText.GetComponent<RectTransform>();
			if (rectTransform == null)
			{
				Debug.Log("SetMarkText Error : null == rectTransform");
				return;
			}

			Text text = markText.GetComponent<Text>();
			if (text == null)
			{
				Debug.Log("SetMarkText Error : null == Text");
				return;
			}

			rectTransform.anchorMin = new Vector2(0, 0);
			rectTransform.anchorMax = new Vector2(0, 0);
			rectTransform.pivot = new Vector2(0, 0);
			rectTransform.anchoredPosition = rect.position;
			rectTransform.sizeDelta = rect.size;

			text.text = str;
			text.enabled = isEnable;
		}

		private void ResetMarkText(GameObject markText)
		{
			SetMarkText(markText, new Rect(new Vector2(0, coordinateRectTransform.rect.y), new Vector2(coordinateRectTransform.rect.x, m_MinMarkTextHeight)), null, false);
		}

		private void ResetAllMarkText(List<GameObject> markTexts)
		{
			if (markTexts == null)
			{
				Debug.Log("DisableAllMarkText Error : null == markTexts");
				return;
			}

			foreach (GameObject g in markTexts)
			{
				ResetMarkText(g);
			}
		}

		private void DrawOneHorizontalMarkText(GameObject markText, float markValY, float pixelY, Rect coordinateRect)
		{
			SetMarkText(markText, new Rect(new Vector2(0, pixelY - (m_MinMarkTextHeight / 2)), new Vector2(coordinateRect.x - 2, m_MinMarkTextHeight)), markValY.ToString(), true);
		}

		private IEnumerator DrawHorizontalTextMark(float[] marksVals, float[] marksPixel, Rect coordinateRect)
		{
			yield return new WaitForSeconds(0);

			while (marksPixel.Length > m_MarkHorizontalTexts.Count)
			{
				GameObject markText = Instantiate(markTextPrefab, transform);
				markText.name = string.Format("{0}{1}", MARK_TEXT_BASE_NAME, m_MarkHorizontalTexts.Count);
				m_MarkHorizontalTexts.Add(markText);
			}

			ResetAllMarkText(m_MarkHorizontalTexts);

			for (int i = 0; i < marksPixel.Length; i++)
			{
				DrawOneHorizontalMarkText(m_MarkHorizontalTexts[i], marksVals[i], marksPixel[i], coordinateRect);
			}

			yield return null;
		}

		private void DrawOneHorizontalMark(VertexHelper vh, float pixelY, Rect coordinateRect)
		{
			Vector2 startPoint = new Vector2(coordinateRect.x, pixelY);
			Vector2 endPoint = new Vector2(coordinateRect.x + coordinateRect.width, pixelY);
			DrawHorizontalSegmet(vh, startPoint, endPoint, markColor, rectThickness / 2);
		}

		private void DrawHorizontalMark(VertexHelper vh, Rect coordinateRect)
		{
			int markNum = CalcMarkNum(m_PixelPerMark, coordinateRect.height);
			float markLevel = CalcMarkLevel(MarkIntervalTab, markNum, coordinateAxisViewSizeY);
			float[] marksVals = CalcMarkVals(markLevel, coordinateAxisViewRange.y, coordinateAxisViewRange.y + coordinateAxisViewSizeY);
			float[] marksPixel = MarkValsToPixel(marksVals, coordinateAxisViewRange.y, coordinateAxisViewRange.y + coordinateAxisViewSizeY, coordinateRect.y, coordinateRect.y + coordinateRect.height);

			for (int i = 0; i < marksPixel.Length; i++)
			{
				DrawOneHorizontalMark(vh, marksPixel[i], coordinateRect);
			}

			StartCoroutine(DrawHorizontalTextMark(marksVals, marksPixel, coordinateRect));
		}

		private void DrawRect(VertexHelper vh, Rect rect)
		{
			DrawRectang(vh, rect.position,
				new Vector2(rect.x, rect.y + rect.height),
				new Vector2(rect.x + rect.width, rect.y + rect.height),
				new Vector2(rect.x + rect.width, rect.y), backgroundColor);
		}

		private void DrawRectCoordinate(VertexHelper vh)
		{
			if (coordinateRectTransform)
			{
				Rect marksRect = new Rect(coordinateRectTransform.offsetMin, coordinateRectTransform.rect.size);
				DrawRect(vh, new Rect(marksRect));
				DrawHorizontalMark(vh, marksRect);
			}
		}

		private void FindExistMarkText(List<GameObject> markTexts)
		{
			foreach (Transform t in transform)
			{
				if (Regex.IsMatch(t.gameObject.name, MARK_TEXT_BASE_NAME))
				{
					t.gameObject.name = string.Format("{0}{1}", MARK_TEXT_BASE_NAME, m_MarkHorizontalTexts.Count);
					markTexts.Add(t.gameObject);
				}

			}
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			DrawRectCoordinate(vh);
		}

		public void InputPoint(GameObject line, Vector2 point)
		{
			line.GetComponent<DrawGraphicLines>().AddPoint(CoordinateToPixel(point));
		}

		public GameObject AddLine(string name)
		{
			if (linesPrefab == null)
			{
				linesPrefab = (GameObject)Resources.Load("Prefabs/Lines");
			}

			try
			{
				lineList.Add(Instantiate(linesPrefab, coordinateRectTransform));
			}
			catch (NullReferenceException e)
			{
				Debug.Log(this + "," + e);
				return null;
			}

			lineList[lineList.Count - 1].GetComponent<DrawGraphicLines>().lineName = name;
			lineList[lineList.Count - 1].GetComponent<DrawGraphicLines>().color = Color.yellow;
			lineList[lineList.Count - 1].name = String.Format("{0}{1}", LINES_BASE_NAME, lineList[lineList.Count - 1].GetComponent<DrawGraphicLines>().lineName);
			return lineList[lineList.Count - 1];
		}

		public bool RemoveLine(GameObject line)
		{
			if (line == null)
			{
				return true;
			}

			if (!lineList.Remove(line))
			{
				return false;
			}
			try
			{
				line.GetComponent<DrawGraphicLines>().Clear();
			}
			catch (NullReferenceException)
			{

			}
			Destroy(line);
			return true;
		}

	}

}
