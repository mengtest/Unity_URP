using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPA
{
	[System.Serializable]
	public class SceneRendererParam
	{
		/// <summary>
		/// RGB→明るさ換算式
		/// </summary>
		[SerializeField, Range(0f, 1f)]
		private float intensity = default;

		/// <summary>
		/// 輝度変化
		/// </summary>
		[SerializeField, Range(0f, 100f)]
		private float luminance = default;

		/// <summary>
		/// 鮮やかな赤
		/// </summary>
		[SerializeField, Range(0f, 100f)]
		private float brightnessR = default;

		/// <summary>
		/// RGB→明るさ換算式（前フレ）
		/// </summary>
		[SerializeField, Range(0f, 1f)]
		private float prevIntensity = default;

		/// <summary>
		/// 輝度変化（前フレ）
		/// </summary>
		[SerializeField, Range(0f, 100f)]
		private float prevLuminance = default;

		/// <summary>
		/// 鮮やかな赤（前フレ）
		/// </summary>
		[SerializeField, Range(0f, 100f)]
		private float prevBrightnessR = default;

		/// <summary>
		/// 画面の平均色
		/// </summary>
		[SerializeField]
		private Color32 averageColor32 = default;


		public float Intensity { get => intensity; }
		public float Luminance { get => luminance; }
		public float BrightnessR { get => brightnessR; }

		public float PrevIntensity { get => prevIntensity; }
		public float PrevLuminance { get => prevLuminance; }
		public float PrevBrightnessR { get => prevBrightnessR; }

		public Color32 AverageColor32 { get => averageColor32; }

		public float LuminanceDiff
		{
			get => Mathf.Clamp(luminance - prevLuminance, 0, 100);
		}

		public SceneRendererParam()
		{

		}

		public void SetIntensity(float newIntensity)
		{
			intensity = newIntensity;
		}

		public void SetLuminance(float newLuminance)
		{
			luminance = newLuminance;
		}

		public void SetBrightnessR(float newBrightnessR)
		{
			brightnessR = newBrightnessR;
		}

		public void SetPrevIntensity(float newPrevIntensity)
		{
			if (prevIntensity != newPrevIntensity)
			{
				prevIntensity = newPrevIntensity;
			}
		}

		public void SetPrevLuminance(float newPrevLuminance)
		{
			if (prevLuminance != newPrevLuminance)
			{
				prevLuminance = newPrevLuminance;
			}
		}

		public void SetPrevBrightnessR(float newPrevBrightnessR)
		{
			if (prevBrightnessR != newPrevBrightnessR)
			{
				prevBrightnessR = newPrevBrightnessR;
			}
		}

		public void SetAverageColor32(Color32 newAverageColor32)
		{
			averageColor32 = newAverageColor32;
		}
	}

}
