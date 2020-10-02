using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPA
{
	// ZoomButton
	public class ZoomButtonClickEventArgs : EventArgs
	{

	}

	// DataDiagram
	public class CalcRectTransformHelper
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

	// DataDiagram
	public class RectChangeEventArgs : EventArgs
	{
		private readonly Vector2 m_Size;

		public RectChangeEventArgs(Vector2 size)
		{
			m_Size = size;
		}

		public Vector2 size { get => m_Size; }
	}

	// DataDiagram
	public class ZoomEventArgs : EventArgs
	{
		private float _zoomX;
		private float _zoomY;

		public ZoomEventArgs(float valX, float valY) : base()
		{
			_zoomX = valX;
			_zoomY = valY;
		}

		public float ZoomX { get => _zoomX; }
		public float ZoomY { get => _zoomY; }
	}

	// DataDiagram
	public class MoveEventArgs : EventArgs
	{
		private float _moveX = 0;
		private float _moveY = 0;

		public MoveEventArgs(float dx, float dy)
		{
			_moveX = dx;
			_moveY = dy;
		}

		public float MoveX { get => _moveX; }
		public float MoveY { get => _moveY; }
	}

	// DataDiagram
	public class PreDestroyLineEventArgs : EventArgs
	{

		GameObject m_Line = null;

		public PreDestroyLineEventArgs(GameObject line)
		{
			m_Line = null;

			if (!line)
			{
				return;
			}

			if (!line.GetComponent<DrawGraphicLines>())
			{
				return;
			}
			m_Line = line;
		}

		public GameObject line { get => m_Line; }
	}

	// CoordinateAxis
	public class CoordinateRectChangeEventArgs : EventArgs
	{
		public Rect viewRectInPixel;

		public CoordinateRectChangeEventArgs(Rect newRect) : base()
		{
			viewRectInPixel = newRect;
		}
	}

	// CoordinateAxis
	public class CoordinateScaleChangeEventArgs : EventArgs
	{
		public float scaleX;
		public float scaleY;

		public CoordinateScaleChangeEventArgs(float inScaleX, float inScaleY) : base()
		{
			scaleX = inScaleX;
			scaleY = inScaleY;
		}
	}

	// CoordinateAxis
	public class CoordinateZeroPointChangeEventArgs : EventArgs
	{
		public Vector2 zeroPoint;

		public CoordinateZeroPointChangeEventArgs(Vector2 zeroPoint) : base()
		{
			this.zeroPoint = zeroPoint;
		}
	}

}

