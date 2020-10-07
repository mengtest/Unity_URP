using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPA
{
	public class SceneRendererView : PersistentSingletonMonoBehaviour<SceneRendererView>
	{
		private SceneRenderer sceneRenderer = default;

		[SerializeField]
		private Text intensity = default;

		[SerializeField]
		private Text prevIntensity = default;

		[SerializeField]
		private Text luminance = default;

		[SerializeField]
		private Text prevLuminance = default;

		[SerializeField]
		private Text luminanceChange = default;

		[SerializeField]
		private Text brightnessR = default;

		[SerializeField]
		private Text prevBrightnessR = default;

		[SerializeField]
		private Image averageColor32 = default;


		public void SetTarget(SceneRenderer newSceneRenderer)
		{
			sceneRenderer = newSceneRenderer;
		}

		private SceneRendererParam param
		{
			get => sceneRenderer.SceneRendererParam;
		}

		private bool CanRenderer()
		{
			if (sceneRenderer == null || sceneRenderer.SceneRendererParam == null)
			{
				return false;
			}
			return true;
		}

		private void Update()
		{
			if (intensity == null || prevIntensity == null || 
				luminance == null || prevLuminance == null || luminanceChange == null ||
				brightnessR == null || prevBrightnessR == null ||
				averageColor32 == null)
			{
				return;
			}

			intensity.text = !CanRenderer() ?  string.Empty : param.Intensity.ToString();
			prevIntensity.text = !CanRenderer() ? string.Empty : param.PrevIntensity.ToString();

			luminance.text = !CanRenderer() ?  string.Empty : param.Luminance.ToString();
			prevLuminance.text = !CanRenderer() ? string.Empty : param.PrevLuminance.ToString();

			float luminanceChangeValue = !CanRenderer() ? 0 : param.LuminanceChange;
			bool wasAlertBrightness = !CanRenderer() ? false : sceneRenderer.WasAlertBrightness;
			luminanceChange.color = wasAlertBrightness ? Color.red : Color.black;
			luminanceChange.text = luminanceChangeValue.ToString();

			brightnessR.text = !CanRenderer() ? string.Empty : param.BrightnessR.ToString();
			prevBrightnessR.text = !CanRenderer() ? string.Empty : param.PrevBrightnessR.ToString();
			averageColor32.color = !CanRenderer() ? Color.white : SceneRendererUtil.ConvertColor(param.AverageColor32);
		}
	}

}
