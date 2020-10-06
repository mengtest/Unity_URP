using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace FPA
{
	[RequireComponent(typeof(Camera))]
	public class SceneRenderer : MonoBehaviour
	{
		private const CameraEvent kCameraEvent = CameraEvent.AfterImageEffects;

		[SerializeField]
		private bool playOnAwake = false;

		[SerializeField]
		private SceneRendererParam sceneRendererParam = new SceneRendererParam();

		private Texture2D targetTexture = default;
		private RenderTexture renderTexture = default;
		private Camera targetCamera = default;
		private Camera childCamera = default;
		private CommandBuffer rendererBuffer = default;
		private bool abort = false;

		public bool Abort { get => abort; }

		public SceneRendererParam SceneRendererParam { get => sceneRendererParam; }

		protected virtual void Start()
		{
			if (playOnAwake)
			{
				OnSceneRender();
			}
		}

		protected virtual void OnDestroy()
		{
			if (targetTexture)
			{
				Destroy(targetTexture);
				targetTexture = null;
			}
			DestroyRenderTexture();
			DestroyChildCamera();
		}

		public void OnSceneRender()
		{
			if (targetCamera == null)
			{
				targetCamera = GetComponent<Camera>();
			}

			if (!targetCamera.targetTexture)
			{
				CreateRenderTexture();
			}

			SceneRendererView.Instance.SetTarget(this);
			StartCoroutine(RendererCoroutine());
		}

		public void OnAbort()
		{
			abort = true;
			SceneRendererView.Instance.SetTarget(null);
		}

		private void CreateRenderTexture()
		{
			DestroyRenderTexture();

			renderTexture = SceneRendererUtil.CreateRenderTexture(24, 
				SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR) ? 
				RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);
			renderTexture.name = "RGB_RT";
			renderTexture.Create();

			targetCamera.targetTexture = renderTexture;
			childCamera = SceneRendererUtil.CreateChildCamera(targetCamera);
			CreateBlitCommand(ref rendererBuffer, childCamera);
		}

		private void CreateBlitCommand(ref CommandBuffer cb, Camera camera)
		{
			cb = new CommandBuffer();
			cb.name = camera.name + " Blit to Back buffer";
			cb.SetRenderTarget(-1);
			cb.Blit(renderTexture, BuiltinRenderTextureType.CameraTarget);
			camera.AddCommandBuffer(kCameraEvent, cb);
		}

		private void DestroyRenderTexture()
		{
			if (renderTexture == null)
			{
				return;
			}
			if (targetCamera)
			{
				targetCamera.targetTexture = null;
			}
			renderTexture.Release();
			Destroy(renderTexture);
			renderTexture = null;
		}

		private void DestroyChildCamera()
		{
			if (childCamera)
			{
				if (rendererBuffer != null)
				{
					childCamera.RemoveCommandBuffer(kCameraEvent, rendererBuffer);
					rendererBuffer = null;
				}
				Destroy(childCamera);
				childCamera = null;
			}
		}

		protected IEnumerator RendererCoroutine()
		{
			var tex = targetCamera.targetTexture;
			targetTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);

			while (!abort)
			{
				sceneRendererParam.SetPrevIntensity(sceneRendererParam.Intensity);
				sceneRendererParam.SetPrevLuminance(sceneRendererParam.Luminance);
				sceneRendererParam.SetPrevBrightnessR(sceneRendererParam.BrightnessR);

				RenderTexture.active = targetCamera.targetTexture;
				yield return new WaitForEndOfFrame();
				targetTexture.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
				targetTexture.Apply();

				var pixels = targetTexture.GetPixels();
				var pixels32 = targetTexture.GetPixels32();
				sceneRendererParam.SetIntensity(SceneRendererUtil.GetLightValue(pixels));
				sceneRendererParam.SetLuminance(SceneRendererUtil.GetLuminance(pixels32));
				sceneRendererParam.SetBrightnessR(SceneRendererUtil.GetBrightnessR(pixels32));
				sceneRendererParam.SetAverageColor32(SceneRendererUtil.GetAverageColor32(pixels32));
				yield return null;
			}
		}

	}

}
