using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPA
{
	public static class SceneRendererUtil
	{
		/// <summary>
		/// Get average pixel Intensity
		/// </summary>
		/// <param name="tex"></param>
		/// <returns></returns>
		public static float GetLightValue(Texture2D tex)
		{
			return GetLightValue(tex.GetPixels());
		}

		public static float GetLightValue(Color[] cols)
		{
			Color pixel = new Color(0, 0, 0);
			foreach (var col in cols)
			{
				pixel += col;
			}
			pixel /= cols.Length;
			return ToYuv(pixel);
		}

		/// <summary>
		/// Pixelの平均色を求め、Color32で返す(各チャンネル0~255)
		/// </summary>
		/// <returns></returns>
		public static Color32 GetAverageColor32(Texture2D tex)
		{
			return GetAverageColor32(tex.GetPixels32());
		}

		public static Color32 GetAverageColor32(Color32[] cols)
		{
			int total = cols.Length;
			float r = 0;
			float g = 0;
			float b = 0;
			float a = 0;

			foreach (var col in cols)
			{
				r += col.r;
				g += col.g;
				b += col.b;
				a += col.a;
			}
			return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), (byte)(a / total));
		}

		/// <summary>
		/// 点滅を行なう面積が画面の1/4を超え,かつ,輝度変化が 10％以上の場合を基準
		/// 輝度(明度)の％は,以下の式にて求める
		/// 輝度(%)＝RGBの中の最大値÷値の上限値(255)×100
		/// </summary>
		/// <param name="tex"></param>
		/// <returns></returns>
		public static float GetLuminance(Texture2D tex)
		{
			return GetLuminance(tex.GetPixels32());
		}

		public static float GetLuminance(Color32[] cols)
		{
			Color32 color = GetAverageColor32(cols);
			float value = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
			return Mathf.Floor((value / 255) * 100);
		}

		/// <summary>
		///「鮮やかな赤」とは、ある色のR値が、その色のRGB値合計の85％を上回る場合をいう。
		/// 光感受性には要因の一つとして波長依存性があり,長波長赤色光(620-750nm)が危険帯とされる。
		/// </summary>
		/// <returns></returns>
		public static float GetBrightnessR(Color32[] cols)
		{
			Color32 color = GetAverageColor32(cols);
			float r = color.r;
			float total = (color.r + color.g + color.b);
			return Mathf.Floor((r / total) * 100);
		}



		public static RenderTexture CreateRenderTexture(int depth = 24, RenderTextureFormat format = RenderTextureFormat.Default)
		{
			int width = (int)(Screen.width);
			int height = (int)(Screen.height);
			return new RenderTexture(width, height, depth, format);
		}

		/// <summary>
		/// https://en.wikipedia.org/wiki/Rec._601
		/// </summary>
		/// <param name="pixel"></param>
		/// <returns></returns>
		public static float ToYuv(Color pixel)
		{
			const float yr = 0.299f;
			const float yb = 0.114f;
			const float yg = 1f - yr - yb;
			float y = (yr * pixel.r) + (yg * pixel.g) + (yb * pixel.b);
			return y;
		}

		public static Camera CreateChildCamera(Camera parentCam)
		{
			GameObject cameraObject = new GameObject("ChildCamera");
			Camera childCamera = cameraObject.AddComponent<Camera>();
			childCamera.transform.SetParent(parentCam.transform, false);
			//childCamera.cullingMask = 0;
			//childCamera.clearFlags = CameraClearFlags.SolidColor;
			//childCamera.depth = parentCam.depth + 1;
			//childCamera.allowMSAA = parentCam.allowMSAA;
			//childCamera.allowHDR = parentCam.allowHDR;
			//childCamera.renderingPath = parentCam.renderingPath;
			childCamera.CopyFrom(parentCam);
			childCamera.depth = parentCam.depth + 1;
			childCamera.targetTexture = null;
			return childCamera;
		}

	}


}

