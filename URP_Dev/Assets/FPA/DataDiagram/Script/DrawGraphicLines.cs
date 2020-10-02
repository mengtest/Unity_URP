using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPA
{

	public class DrawGraphicLines : DiagramDrawGraphic
	{
		private float thickness = 5f;
		private bool isShow = true;
		private bool cursolIsShow = true;

		private List<Vector2> pointList = new List<Vector2>();
		private int cursolStartPointSN = 0;

		private DataDiagram dataDiagram = default;
		private CoordinateAxis coordinate = default;

		[NonSerialized]
		public string lineName = "";

		public float Thickness
		{
			get => thickness;
			set => thickness = value;
		}

		public bool IsShow
		{
			get => isShow;
			set
			{
				if (value != isShow)
				{
					UpdateGeometry();
				}
				isShow = value;
			}
		}

		protected override void Awake()
		{
			dataDiagram = GetComponentInParent<DataDiagram>();
			if (dataDiagram == null)
			{
				Debug.Log(this + "null == m_DataDiagram");
			}

			coordinate = GetComponentInParent<CoordinateAxis>();
			if (coordinate == null)
			{
				Debug.Log(this + "null == m_Coordinate");
			}

			GameObject parent = gameObject.transform.parent.gameObject;
			if (parent == null)
			{
				Debug.Log(this + "null == parent");
			}

			RectTransform parentrt = parent.GetComponent<RectTransform>();
			RectTransform localrt = gameObject.GetComponent<RectTransform>();
			if ((localrt == null) || (parentrt == null))
			{
				Debug.Log(this + "null == localrt || parentrt");
			}

			localrt.anchorMin = Vector2.zero;
			localrt.anchorMax = new Vector2(1, 1);
			localrt.pivot = Vector2.zero;
			localrt.anchoredPosition = Vector2.zero;
			localrt.sizeDelta = Vector2.zero;

			if (coordinate != null)
			{
				coordinate.CoordinateRectChangeEvent += OnCoordinateRectChange;
				coordinate.CoordinateScaleChangeEvent += OnCoordinateScaleChange;
				coordinate.CoordinateeZeroPointChangeEvent += OnCoordinateZeroPointChange;
			}
		}

		private void Update()
		{
			if (cursolIsShow == isShow)
			{
				return;
			}
			cursolIsShow = isShow;
			UpdateGeometry();
		}

		private float ScaleX(float x)
		{
			if (coordinate == null)
			{
				Debug.Log(this + "null == m_Coordinate");
				return x;
			}
			return (x / coordinate.coordinateAxisViewRangeInPixel.width);
		}

		private float ScaleY(float y)
		{
			if (coordinate == null)
			{
				Debug.Log(this + "null == m_Coordinate");
				return y;
			}
			return (y / coordinate.coordinateAxisViewRangeInPixel.height);
		}

		private int GetStartPointSN(List<Vector2> points, float startX)
		{
			int ret = 0;
			float x = 0;
			foreach (Vector2 p in points)
			{
				if (x > startX)
				{
					return points.IndexOf(p);
				}
				x += p.x;
				ret++;
			}
			return ret;
		}

		private void OnCoordinateRectChange(object sender, CoordinateRectChangeEventArgs e)
		{
			UpdateGeometry();
		}

		private void OnCoordinateScaleChange(object sender, CoordinateScaleChangeEventArgs e)
		{
			UpdateGeometry();
		}

		private void OnCoordinateZeroPointChange(object sender, CoordinateZeroPointChangeEventArgs e)
		{
			cursolStartPointSN = GetStartPointSN(pointList, coordinate.coordinateAxisViewRangeInPixel.x);
			UpdateGeometry();
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			if (!isShow)
			{
				return;
			}

			float x = 0;
			List<Vector2> points = new List<Vector2>();
			for (int i = cursolStartPointSN; i < pointList.Count; i++)
			{
				points.Add(new Vector2(ScaleX(x), ScaleY(pointList[i].y - coordinate.coordinateAxisViewRangeInPixel.y)));
				x += pointList[i].x;
				if (x >= coordinate.coordinateAxisViewRangeInPixel.width * rectTransform.rect.width)
				{
					break;
				}
			}
			DrawHorizontalLine(vh, points, color, thickness, new Rect(0, 0, rectTransform.rect.width, rectTransform.rect.height));
		}

		public void AddPoint(Vector2 point)
		{
			pointList.Insert(0, new Vector2(point.x, point.y));
			while (pointList.Count > dataDiagram.maxPointNum)
			{
				pointList.RemoveAt(pointList.Count - 1);
				print(pointList.Count);
			}
			UpdateGeometry();
		}

		public bool Clear()
		{
			if (coordinate == null)
			{
				Debug.LogWarning(this + "null == m_Coordinate");
			}

			try
			{
				coordinate.CoordinateRectChangeEvent -= OnCoordinateRectChange;
				coordinate.CoordinateScaleChangeEvent -= OnCoordinateScaleChange;
				coordinate.CoordinateeZeroPointChangeEvent -= OnCoordinateZeroPointChange;
				pointList.Clear();
			}
			catch (NullReferenceException e)
			{
				Debug.LogError(this + " : " + e);
				return false;
			}
			return true;
		}
	}

}

