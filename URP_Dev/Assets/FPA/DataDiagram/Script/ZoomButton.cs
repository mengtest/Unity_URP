using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPA
{

	public class ZoomButton : MonoBehaviour
	{
		public delegate void ZoomButtonClickHandle(object sender, ZoomButtonClickEventArgs args);
		public ZoomButtonClickHandle ZoomButtonClickEvent;

		[SerializeField]
		private Button clickButton = default;

		private DataDiagram dataDiagram = default;

		struct RTParam
		{

			public Transform parent;
			public Rect rect;
		}

		private RTParam[] rectParams = new RTParam[2];
		private int paramSN = 0;


		private void Start()
		{
			dataDiagram = GetComponentInParent<DataDiagram>();
			if (dataDiagram == null)
			{
				Debug.LogWarning(this + "Awake Error : null == m_DataDiagram");
				return;
			}

			RectTransform rt = dataDiagram.GetComponent<RectTransform>();
			rectParams[0].parent = dataDiagram.transform.parent;
			rectParams[0].rect = CalcRectTransformHelper.CalcLocalRect(rt.anchorMin, rt.anchorMax, rectParams[0].parent.GetComponent<RectTransform>().rect.size, rt.pivot, rt.anchoredPosition, rt.rect);
			rectParams[1].parent = GetComponentInParent<Canvas>().transform;
			rectParams[1].rect = new Rect(new Vector2(Screen.width / 10, Screen.height / 10), new Vector2(Screen.width * 8 / 10, Screen.height * 8 / 10));
			paramSN = 0;


			clickButton?.onClick.AddListener(OnZoomButton);
		}


		public void OnZoomButton()
		{
			if (dataDiagram == null)
			{
				return;
			}

			paramSN = (paramSN + 1) % 2;
			dataDiagram.transform.SetParent(rectParams[paramSN].parent);
			dataDiagram.Rect = rectParams[paramSN].rect;
			ZoomButtonClickEvent(this, new ZoomButtonClickEventArgs());
		}

	}

}
