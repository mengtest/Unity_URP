using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FPA
{

	public class DragBar : MonoBehaviour, IDragHandler
	{
		private ZoomButton zoomButton = default;
		private GameObject dataDiagramGO = default;
		private GameObject parentGO = default;
		private RectTransform dataDiagramRT = default;

		public bool CanDrag
		{
			get => gameObject.activeSelf;

			set
			{
				LayoutElement element = GetComponent<LayoutElement>();
				if (element == null)
				{
					Debug.LogWarning(this + " : can not find LayoutElement");
					return;
				}

				else
				{
					gameObject.SetActive(value ? true : false);
					element.ignoreLayout = value ? false : true;
				}
			}
		}

		private void Start()
		{
			GetZoomButton();
			DataDiagram diagram = GetComponentInParent<DataDiagram>();
			if (diagram == null)
			{
				Debug.LogWarning(this + " : can not find any gameobject with a DataDiagram object");
				return;
			}
			else
			{
				dataDiagramGO = diagram.gameObject;
			}

			dataDiagramRT = dataDiagramGO.GetComponent<RectTransform>();
			if (!dataDiagramGO.transform.parent)
			{
				parentGO = null;
			}
			else
			{
				parentGO = dataDiagramGO.transform.parent.gameObject;
			}

			if (!parentGO)
			{
				Debug.LogWarning(this + " : can not DataDiagram's parent");
				return;
			}

			CanDrag = (parentGO.GetComponent<Canvas>() == null) ? false : true;
		}

		private void GetZoomButton()
		{
			if (zoomButton == null)
			{
				GameObject g = GameObject.Find("ZoomButton");
				if (g == null)
				{
					Debug.LogWarning(this + " : can not find gameobject ZoomButton");
					return;
				}
				else
				{
					if (g.GetComponentInParent<DataDiagram>() == null)
					{
						Debug.LogWarning(this + " : the gameobject ZoomButton is not under the DataDiagram");
						return;
					}
					else
					{
						zoomButton = g.GetComponent<ZoomButton>();
						if (zoomButton == null)
						{
							Debug.LogWarning(this + " : can not find object DD_ZoomButton");
							return;
						}
						else
						{
							zoomButton.ZoomButtonClickEvent += OnCtrlButtonClick;
						}
					}
				}
			}
			else
			{
				zoomButton.ZoomButtonClickEvent += OnCtrlButtonClick;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (dataDiagramRT)
			{
				dataDiagramRT.anchoredPosition += eventData.delta;
			}
		}

		private void OnCtrlButtonClick(object sender, ZoomButtonClickEventArgs e)
		{
			if (dataDiagramGO.transform.parent == null)
			{
				Debug.LogWarning(this + " OnCtrlButtonClick : can not DataDiagram's parent");
				return;
			}

			if (parentGO != dataDiagramGO.transform.parent.gameObject)
			{
				parentGO = dataDiagramGO.transform.parent.gameObject;
				CanDrag = (parentGO.GetComponent<Canvas>() != null) ? true : false;
			}
		}
	}

}
