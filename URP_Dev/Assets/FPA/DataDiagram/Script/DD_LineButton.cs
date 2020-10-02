using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace FPA
{
	public class DD_LineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private GameObject line = default;
		public GameObject labelGO = default;

		public GameObject Line
		{
			get => line;
			set
			{
				DD_Lines lines = value.GetComponent<DD_Lines>();
				if (lines == null)
				{
					Debug.LogWarning(this.ToString() + "LineButton error : set line null == value.GetComponent<Lines>()");
					return;
				}
				else
				{
					line = value;
					SetLineButton(lines);
				}
			}
		}

		private void SetLabel(DD_Lines lines)
		{
			if ((labelGO == null) || (labelGO.GetComponent<Text>() == null))
			{
				return;
			}

			try
			{
				labelGO.GetComponent<Text>().text = lines.GetComponent<DD_Lines>().lineName;
				labelGO.GetComponent<Text>().color = lines.GetComponent<DD_Lines>().color;
			}
			catch
			{
				labelGO.GetComponent<Text>().color = Color.white;
			}
		}

		public void SetLineButton(DD_Lines lines)
		{
			name = string.Format("Button{0}", lines.gameObject.name);
			GetComponent<Image>().color = lines.color;
			SetLabel(lines);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject != gameObject)
			{
				return;
			}

			if (labelGO == null)
			{
				return;
			}
			DD_DataDiagram dd = GetComponentInParent<DD_DataDiagram>();
			if (dd == null)
			{
				return;
			}

			labelGO.transform.SetParent(dd.transform);
			labelGO.transform.position = transform.position + new Vector3(0, -GetComponent<RectTransform>().rect.height / 2, 0);
			labelGO.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (labelGO == null)
			{
				return;
			}
			labelGO.transform.SetParent(transform);
			labelGO.SetActive(false);
		}

		public void OnButtonClick()
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				return;
			}

			if (line == null)
			{
				Debug.LogWarning(this.ToString() + "error OnButtonClick : null == m_Line");
				return;
			}

			DD_Lines lines = line.GetComponent<DD_Lines>();
			if (lines == null)
			{
				Debug.LogWarning(this.ToString() + "error OnButtonClick : null == DD_Lines");
				return;
			}
			else
			{
				lines.IsShow = !lines.IsShow;
			}
		}

		public void OnButtonClickWithCtrl()
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				try
				{
					transform.GetComponentInParent<DD_DataDiagram>().DestroyLine(line);
				}
				catch (NullReferenceException)
				{
					Debug.LogWarning("OnButtonClickWithCtrl throw a NullReferenceException");
				}
			}
		}

		public void DestroyLineButton()
		{
			if (labelGO)
			{
				Destroy(labelGO);
			}
		}
	}

}

